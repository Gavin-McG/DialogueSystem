using UnityEngine;
using System.Collections.Generic;
using NUnit.Framework;
using WolverineSoft.DialogueSystem.Values;
using Newtonsoft.Json;


namespace WolverineSoft.DialogueSystem.Tests
{

    public class ValueSaveTests
    {
        // Simple IValueContext test stub (must match project's IValueContext)
        private class TestContext : IValueContext
        {
            public string ContextName { get; }
            public TestContext(string name) => ContextName = name;
        }

        private DSValue CreateValue(string name)
        {
            var v = ScriptableObject.CreateInstance<DSValue>();
            v.valueName = name;
            return v;
        }

        private ValueHolder CreateHolder(params DSValue[] values)
        {
            var holder = ScriptableObject.CreateInstance<ValueHolder>();
            holder.SetValues(new List<DSValue>(values));
            return holder;
        }

        // Helper to JSON round-trip any object while preserving types (polymorphism)
        private T DeepCloneViaJson<T>(T obj)
        {
            var settings = new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.All,
                Formatting = Formatting.Indented
            };
            var json = JsonConvert.SerializeObject(obj, settings);
            return JsonConvert.DeserializeObject<T>(json, settings);
        }

        [Test]
        public void DSValue_SaveRestore_MultipleScopesAndTypes_CorrectlyRestoresLowestScopeAndTypes()
        {
            var ctxA = new TestContext("ContextA");
            var ctxB = new TestContext("ContextB");
            var ctxC = new TestContext("ContextC");

            var ds = CreateValue("TestValue");

            // ContextA: Global = 100 (int), Manager = 50 (int) -> lowest local scope present is Manager (50)
            ds.SetValue(ctxA, DSValue.ValueScope.Global, 100);
            ds.SetValue(ctxA, DSValue.ValueScope.Manager, 50);

            // ContextB: Global = 10f (float), Dialogue = "alive" (string) -> Dialogue is lowest -> "alive"
            ds.SetValue(ctxB, DSValue.ValueScope.Global, 10f);
            ds.SetValue(ctxB, DSValue.ValueScope.Dialogue, "alive");

            // ContextC: only Global = true (bool)
            ds.SetValue(ctxC, DSValue.ValueScope.Global, true);

            // Save -> JSON roundtrip
            var save = ds.GetSaveData();
            var clonedSave = DeepCloneViaJson(save);
            
            // Restore into a fresh DSValue instance with the same name
            var restored = CreateValue("TestValue");
            restored.RestoreFromSave(clonedSave);

            // --- Context A expectations ---
            // Lowest scope is Manager (an int 50)
            Assert.AreEqual(DSValue.ValueScope.Manager, restored.GetValueScope(ctxA),
                "GetValueScope should report Manager for ctxA because a Manager value was set.");

            // Typed retrieval: int must match exactly
            Assert.IsTrue(restored.TryGetValue<int>(ctxA, out var intA), "Typed int retrieval should succeed for ctxA.");
            Assert.AreEqual(50, intA);

            // Typed retrieval: float (generic) should fail because stored value is an int (exact-type required)
            Assert.IsFalse(restored.TryGetValue<float>(ctxA, out _), "Generic TryGetValue<float> should fail when the stored value is an int.");

            // Numeric retrieval with the float overload should succeed (IConvertible conversion)
            Assert.IsTrue(restored.TryGetValue(ctxA, out float floatA), "TryGetValue(... out float) should convert int -> float.");
            Assert.AreEqual(50f, floatA);

            // --- Context B expectations ---
            // Lowest scope is Dialogue (string)
            Assert.AreEqual(DSValue.ValueScope.Dialogue, restored.GetValueScope(ctxB),
                "GetValueScope should report Dialogue for ctxB because a Dialogue value was set.");

            Assert.IsTrue(restored.TryGetValue<string>(ctxB, out var sB), "Typed string retrieval should succeed for ctxB.");
            Assert.AreEqual("alive", sB);

            // Trying to get a numeric float from a string should fail
            Assert.IsFalse(restored.TryGetValue(ctxB, out float _), "Numeric TryGetValue(out float) should fail for non-convertible string.");

            // --- Context C expectations ---
            // Global bool present, no locals -> Global should be returned
            Assert.AreEqual(DSValue.ValueScope.Global, restored.GetValueScope(ctxC));
            Assert.IsTrue(restored.TryGetValue<bool>(ctxC, out var bC));
            Assert.IsTrue(bC);
        }

        [Test]
        public void ValueHolder_SaveRestore_MultipleValues_RestoresEachValueByName()
        {
            var ctx = new TestContext("Gameplay");

            // Prepare values with varied types and scopes
            var vGold = CreateValue("Gold");
            vGold.SetValue(ctx, DSValue.ValueScope.Global, 250); // int

            var vName = CreateValue("PlayerName");
            vName.SetValue(ctx, DSValue.ValueScope.Dialogue, "Hero"); // string (Dialogue scope)

            var vAlive = CreateValue("IsAlive");
            vAlive.SetValue(ctx, DSValue.ValueScope.Manager, true); // bool (Manager scope)

            var holder = CreateHolder(vGold, vName, vAlive);

            // Save -> JSON roundtrip
            var savedHolder = holder.GetSaveData();
            var cloned = DeepCloneViaJson(savedHolder);

            // Create fresh DSValues with the same names and put them into a new holder
            var newGold = CreateValue("Gold");
            var newName = CreateValue("PlayerName");
            var newAlive = CreateValue("IsAlive");
            var newHolder = CreateHolder(newGold, newName, newAlive);

            // Restore
            newHolder.RestoreFromSave(cloned);

            // Validate restored values
            Assert.IsTrue(newGold.TryGetValue<int>(ctx, out var goldVal));
            Assert.AreEqual(250, goldVal);

            Assert.IsTrue(newName.TryGetValue<string>(ctx, out var playerName));
            Assert.AreEqual("Hero", playerName);

            Assert.IsTrue(newAlive.TryGetValue<bool>(ctx, out var aliveVal));
            Assert.IsTrue(aliveVal);
        }
    }
}