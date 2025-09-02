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

        private DSValueHolder CreateHolder(params DSValue[] values)
        {
            var holder = ScriptableObject.CreateInstance<DSValueHolder>();
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
            var save = ds.GetData();
            var clonedSave = DeepCloneViaJson(save);
            
            // Restore into a fresh DSValue instance with the same name
            var restored = CreateValue("TestValue");
            restored.RestoreFromData(clonedSave);

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
            var savedHolder = holder.GetData();
            var cloned = DeepCloneViaJson(savedHolder);

            // Create fresh DSValues with the same names and put them into a new holder
            var newGold = CreateValue("Gold");
            var newName = CreateValue("PlayerName");
            var newAlive = CreateValue("IsAlive");
            var newHolder = CreateHolder(newGold, newName, newAlive);

            // Restore
            newHolder.RestoreFromData(cloned);

            // Validate restored values
            Assert.IsTrue(newGold.TryGetValue<int>(ctx, out var goldVal));
            Assert.AreEqual(250, goldVal);

            Assert.IsTrue(newName.TryGetValue<string>(ctx, out var playerName));
            Assert.AreEqual("Hero", playerName);

            Assert.IsTrue(newAlive.TryGetValue<bool>(ctx, out var aliveVal));
            Assert.IsTrue(aliveVal);
        }
        
        [Test]
        public void DSValue_SaveDataString_RoundTrip_Works()
        {
            var ctx = new TestContext("Quest");

            var value = CreateValue("QuestProgress");
            value.SetValue(ctx, DSValue.ValueScope.Global, 3);

            // Save directly to string
            string saveString = value.GetSaveData();

            // New DSValue, restore from string
            var restored = CreateValue("QuestProgress");
            restored.RestoreFromSaveData(saveString);

            Assert.IsTrue(restored.TryGetValue<int>(ctx, out var restoredVal));
            Assert.AreEqual(3, restoredVal);
        }

        [Test]
        public void ValueHolder_SaveDataString_RoundTrip_Works()
        {
            var ctx = new TestContext("Gameplay");

            var v1 = CreateValue("Coins");
            v1.SetValue(ctx, DSValue.ValueScope.Global, 99);

            var v2 = CreateValue("PlayerTitle");
            v2.SetValue(ctx, DSValue.ValueScope.Manager, "Champion");

            var holder = CreateHolder(v1, v2);

            // Save directly to string
            string saveString = holder.GetSaveData();

            // New holder with new DSValue instances
            var newV1 = CreateValue("Coins");
            var newV2 = CreateValue("PlayerTitle");
            var newHolder = CreateHolder(newV1, newV2);

            newHolder.RestoreFromSaveData(saveString);

            Assert.IsTrue(newV1.TryGetValue<int>(ctx, out var coins));
            Assert.AreEqual(99, coins);

            Assert.IsTrue(newV2.TryGetValue<string>(ctx, out var title));
            Assert.AreEqual("Champion", title);
        }

        [Test]
        public void DSValue_RestoreFromSaveData_IgnoresMismatchedId()
        {
            var ctx = new TestContext("Test");

            var value = CreateValue("CorrectName");
            value.SetValue(ctx, DSValue.ValueScope.Global, 42);

            // Save string
            string saveString = value.GetSaveData();

            // Create a DSValue with a different name
            var wrong = CreateValue("WrongName");
            wrong.RestoreFromSaveData(saveString);

            // Should not restore (id mismatch), so no value is set
            Assert.IsFalse(wrong.TryGetValue<int>(ctx, out _));
        }

        [Test]
        public void ValueHolder_RestoreFromSaveData_PartiallyRestores()
        {
            var ctx = new TestContext("Partial");

            var v1 = CreateValue("HP");
            v1.SetValue(ctx, DSValue.ValueScope.Global, 100);

            var v2 = CreateValue("XP");
            v2.SetValue(ctx, DSValue.ValueScope.Global, 10);

            var holder = CreateHolder(v1, v2);

            // Save string
            string saveString = holder.GetSaveData();

            // New holder has only one matching value (HP)
            var newHP = CreateValue("HP");
            var holder2 = CreateHolder(newHP); // XP missing

            holder2.RestoreFromSaveData(saveString);

            // HP restored, XP ignored gracefully
            Assert.IsTrue(newHP.TryGetValue<int>(ctx, out var hp));
            Assert.AreEqual(100, hp);
        }

        [Test]
        public void DSValue_RestoreFromSaveData_WithCorruptedJson_DoesNotCrash()
        {
            var value = CreateValue("CorruptTest");
            // Bad JSON input
            string badJson = "{ not valid json...";

            Assert.DoesNotThrow(() =>
            {
                value.RestoreFromSaveData(badJson);
            }, "RestoreFromSaveData should handle bad JSON without throwing.");
        }
    }
}