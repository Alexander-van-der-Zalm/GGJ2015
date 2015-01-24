using UnityEngine;
using System.Collections;

public class MaterialManager : Singleton<MaterialManager> {


	public Material[] matIndex;
	public int size;

	void Start(){
		size = matIndex.Length;
	}
}
