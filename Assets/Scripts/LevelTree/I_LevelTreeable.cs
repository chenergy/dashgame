using UnityEngine;
using System.Collections;

public interface I_LevelTreeable
{
	I_LevelTreeable GetParent();
	I_LevelTreeable[] GetChildren();
	void Unlock();
	void Select();
}

