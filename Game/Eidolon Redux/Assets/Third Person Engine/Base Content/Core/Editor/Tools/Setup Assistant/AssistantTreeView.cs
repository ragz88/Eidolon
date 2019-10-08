/* ==================================================================
   ---------------------------------------------------
   Project   :    Third Person Engine
   Publisher :    Renowned Studio
   Author    :    Tamerlan Favilevich
   ---------------------------------------------------
   Copyright © Tamerlan Favilevich 2017 - 2019 All rights reserved.
   ================================================================== */

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.IMGUI.Controls;

namespace ThirdPersonEngine.Editor
{
	public class AssistantTreeView : TreeView
	{
		private TreeViewItem root;
		private List<TreeViewItem> allItems;

		public AssistantTreeView(TreeViewState treeViewState, AssistantItem[] itemProperties) : base(treeViewState)
		{
			InitializeTreeViewItems(itemProperties);
			SetupParentsAndChildrenFromDepths(root, allItems);
			Reload();
		}

		protected override TreeViewItem BuildRoot()
		{
			if (root == null)
				root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
			return root;
		}

		protected virtual void InitializeTreeViewItems(AssistantItem[] items)
		{
			root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
			allItems = new List<TreeViewItem>();

			string[] types = Enum.GetNames(typeof(AssastantSelection));
			int lastID = 0;
			for (int i = 0; i < types.Length; i++)
			{
				AssistantItem[] item = items.Where(t => t.GetSection().ToString() == types[i]).OrderBy(t => t.GetDisplayName()).OrderBy(t => t.GetPosition()).ToArray();
				if (item == null || item.Length == 0)
				{
					continue;
				}

				for (int j = 0, length = item.Length; j < length; j++)
				{
					item[j].SetID((j + 1) + lastID);
				}
				lastID = item[item.Length - 1].GetID();

				List<TreeViewItem> itemsTreeView = new List<TreeViewItem>() { new TreeViewItem { id = -(i + 1), depth = 0, displayName = types[i] } };

				for (int j = 0; j < item.Length; j++)
				{
					if (item[j].GetDisplayName() == types[i])
					{
						itemsTreeView[0].id = item[j].GetID();
						continue;
					}
					itemsTreeView.Add(new TreeViewItem { id = item[j].GetID(), depth = 1, displayName = string.Format("{0} {1}", item[j].GetDisplayName(), GetPrefix(item[j].IsReady())) });
				}
				allItems.AddRange(itemsTreeView);
			}
		}

		/// <summary>
		/// Generate prefix by item IsReady value.
		/// </summary>
		private string GetPrefix(int value)
		{
			if (value == 1)
				return "✔";
			else if (value == -1)
				return "✘";
			return "";
		}
	}
}