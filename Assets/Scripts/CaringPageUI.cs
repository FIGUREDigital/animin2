using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CaringPageUI : MonoBehaviour {

	public MenuFunctionalityUI menu;

	static public Dictionary<MenuFunctionalityUI, CaringPageUI> menus = new Dictionary<MenuFunctionalityUI, CaringPageUI>();

	public static GameObject GetUI(MenuFunctionalityUI menu)
	{
		CaringPageUI ui;
		if (menus.TryGetValue(menu, out ui))
		{
			return ui.gameObject;
		}
		return null;
	}

	void Awake()
	{
		menus.Add (menu, this);
	}

	void OnDestroy()
	{
		menus.Remove(menu);
	}
}
