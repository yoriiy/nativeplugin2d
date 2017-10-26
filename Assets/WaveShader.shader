// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "WaveShader"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}// ベーステクスチャ
		_Speed("Speed", Range(1, 60)) = 10	// 波の速度
		_Power("Poewr", Range(0, 1)) = .5	// 波の強さ
	}
		SubShader
	{
		// Z値テクスチャ処理はしない
		Cull Off ZWrite Off ZTest Always

		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

	// インプット用構造体
	struct appdata
	{
		float4 vertex : POSITION;
		float2 uv : TEXCOORD0;
	};

	// アウトプット用構造体
	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
	};

	v2f vert(appdata v)
	{
		v2f o;
		// 描画空間(射影、ビュー)とワールド行列掛け合わせ
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;	// UV座標
		return o;
	}


	//	波を作成する
	// @param uv 見ているUV位置
	// @param emitter 波を出すためのUV位置
	// @param speed 波の速度
	// @param phase 波の強さ
	float wave(float2 uv, float2 emitter, float speed, float phase) {
		// ピクセルUV位置と波を出すためのUV位置の距離を出して
		// 時間と速度を掛けて波の速度を出しテクスチャUVがぶれていく
		// べき乗は2.0で２乗してはっきりした強さを決める
		float dst = distance(uv, emitter);
		return pow((0.5 + 0.5 * sin(dst * phase - _Time.y * speed)), 2.0);
	}


	//
	sampler2D _MainTex;
	float _Speed;
	float _Power;

	fixed4 frag(v2f i) : SV_Target
	{
		float2 position = i.uv;
		// 波の振れの作成
		float w = wave(position, float2(0.5, 0.5), _Speed, 200.0);
		w += wave(position, float2(0.6, 0.11), _Speed, 20.0);
		w += wave(position, float2(0.9, 0.6), _Speed, 90.0);
		w = wave(position, float2(0.1, 0.84), _Speed, 150.0);
		w += wave(position, float2(0.32, 0.81), _Speed, 150.0);
		w += wave(position, float2(0.16, 0.211), _Speed, 150.0);
		w += wave(position, float2(0.39, 0.46), _Speed, 150.0);
		w += wave(position, float2(0.51, 0.484), _Speed, 150.0);
		w += wave(position, float2(0.732, 0.91), _Speed, 150.0);
		// 波の強弱をつける
		w *= 0.116 * _Power;
		// UVに波の揺れ分を加算
		i.uv += w;
		// テクスチャに反映
		return tex2D(_MainTex, i.uv);

	}
		ENDCG
	}
	}
}