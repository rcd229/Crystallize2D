using UnityEngine;
using System.Collections;

public class AppearanceGameData  {
	
	public AppearanceGenderData Gender{get; set;}
	public HairDataRef Hair{get; set;}
	public EyeDataRef Eye{get; set;}
	public BodyDataRef Body{get;set;}
	public ShirtDataRef Shirt{get;set;}
	public ShortDataRef Short{get;set;}

	public AppearanceGameData(
		AppearanceGenderData gender, 
		HairDataRef hair, 
		EyeDataRef eye, 
		BodyDataRef body,
		ShirtDataRef shirt,
		ShortDataRef shorts
		)
	{
		Gender = gender;
		Hair = hair;
		Eye = eye;
		Body = body;
		Shirt = shirt;
		Short = shorts;
	}

	public AppearanceGameData(){
		Gender = null;
		Hair = null;
		Eye = null;
		Body = null;
		Shirt = null;
		Short = null;
	}
}
