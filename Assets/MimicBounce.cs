using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MimicBounce : BounceText {

	public BounceText originalBounce;

	protected override void Awake()
	{
		base.Awake();
		this.finalAmplitude1 = originalBounce.finalAmplitude1;
		this.finalTime1 = originalBounce.finalTime1;
		this.finalAmplitude2 = originalBounce.finalAmplitude2;
		this.finalTime2 = originalBounce.finalTime2;
	}
}
