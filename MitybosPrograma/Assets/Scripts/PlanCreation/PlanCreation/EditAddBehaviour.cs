using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class EditAddBehaviour : MonoBehaviour
{
	protected TMP_Text InfoText { get; set; }

	private GameObject parent { get; set; }

	private void Start()
	{
		parent = transform.parent.gameObject;
	}
	public void OnEditButtonClick()
	{
		InfoText = GetComponentInChildren<TMP_Text>();
		InfoText.text = "Ieškomas maistas...";
	}

	public void OnRemoveButtonClick()
	{
		Destroy(parent);
	}

}
