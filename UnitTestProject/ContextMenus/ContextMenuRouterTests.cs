using Hymma.Solidworks.Addins.ContextMenus;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTestProject.ContextMenus
{
    [TestClass]
    public class ContextMenuRouterTests
    {
        [TestMethod]
        public void Execute_InvokesCallback()
        {
            var router = new ContextMenuRouter();
            var called = false;
            var item = new ContextMenuItem("Test", _ => called = true);

            router.Register("1", item);
            router.Execute("1", null);

            Assert.IsTrue(called);
        }

        [TestMethod]
        public void Enable_UsesPredicate()
        {
            var router = new ContextMenuRouter();
            var item = new ContextMenuItem("Test", _ => { }, ctx => ctx.Model == null);

            router.Register("1", item);
            var enabled = router.Enable("1", null);

            Assert.AreEqual(1, enabled);
        }

        [TestMethod]
        public void Enable_ReturnsDisabledWhenMissingToken()
        {
            var router = new ContextMenuRouter();
            var enabled = router.Enable("missing", null);

            Assert.AreEqual(0, enabled);
        }
    }
}
