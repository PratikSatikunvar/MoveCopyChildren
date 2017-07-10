using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Shell.Framework.Commands;
using System;
using System.Collections.Generic;

namespace Demo.POC.Extension
{
    [Serializable]
    public class CopyChildrenTo : Command
    {
        /// <summary>Executes the command in the specified context.</summary>
        /// <param name="context">The context.</param>
        public override void Execute(CommandContext context)
        {
            CopyChildren(context.Items);
        }

        /// <summary>Queries the state of the command.</summary>
        /// <param name="context">The context.</param>
        /// <returns>The state of the command.</returns>
        public override CommandState QueryState(CommandContext context)
        {
            Error.AssertObject((object)context, "context");
            if (context.Items.Length != 1)
                return CommandState.Disabled;
            Item obj = context.Items[0];
            if (obj.Appearance.ReadOnly || !obj.Access.CanRead() || !context.Items[0].Access.CanWriteLanguage())
                return CommandState.Disabled;
            return base.QueryState(context);
        }

        /// <summary>Deletes the children.</summary>
        /// <param name="items">The items.</param>
        /// <param name="message">The message.</param>
        public static void CopyChildren(Item[] items)
        {
            Assert.ArgumentNotNull((object)items, "items");
            if (items.Length == 0)
                return;
            List<Item> objList = new List<Item>();
            foreach (Item obj in items)
            {
                objList.AddRange((IEnumerable<Item>)obj.Children.ToArray());
            }
            Sitecore.Shell.Framework.Items.CopyTo(objList.ToArray());
        }
    }
}
