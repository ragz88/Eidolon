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
	public class PMTreeView : TreeView
	{
		private TreeViewItem root;
		private List<TreeViewItem> allItems;

		public PMTreeView(TreeViewState treeViewState, ManagerItem[] itemProperties) : base(treeViewState)
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

		protected virtual void InitializeTreeViewItems(ManagerItem[] items)
		{
			root = new TreeViewItem { id = 0, depth = -1, displayName = "Root" };
			allItems = new List<TreeViewItem>();

			string[] types = Enum.GetNames(typeof(ManagerSection));
			int lastID = 0;
			for (int i = 0; i < types.Length; i++)
			{
				ManagerItem[] item = items.Where(t => t.GetSection().ToString() == types[i]).OrderBy(t => t.GetDisplayName()).OrderBy(t => t.GetPriority()).ToArray();
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
					itemsTreeView.Add(new TreeViewItem { id = item[j].GetID(), depth = 1, displayName = item[j].GetDisplayName() });
				}
				allItems.AddRange(itemsTreeView);
			}
		}
	}
}