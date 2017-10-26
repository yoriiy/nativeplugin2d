// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/FadeShader" {
	Properties {
		_Color("Tint", Color) = (1,1,1,1)	// 色設定
		[MaterialToggle] PixelSnap("Pixel snap", Float) = 0	// ピクセル移動
	}
	SubShader {
		Tags {
			"Queue" = "Overlay"				// 描画順番設定	Overlay:常に前面
			"RenderType" = "Transparent"	// 描画設定 Transparent:透過シェーダ設定
			"PreviewType" = "Plane"			// 描画方法 Plane:2D表示
			"CanUseSpriteAtlas" = "False"	// 分割表示	False:分割表示しない
		}
		Cull Back					// ポリゴンの裏を描画しない
		Lighting Off				// ライトを適応しない
		ZWrite Off					// Zバッファ書き込みなし
		Blend One OneMinusSrcAlpha	// そのまま(RGB)の色からアルファ値を引いたブレンディング
		
		Pass
		{
		//======= Cgプログラム開始 =======//
		CGPROGRAM
// マクロ定義
#pragma vertex vert				// 頂点処理シェーダ定義
#pragma fragment frag			// ピクセル処理シェーダ定義
#pragma multi_compile _ PIXELSNAP_ON
#include "UnityCG.cginc"		// Unity関数クラス

		// 頂点データから設定する構造体
		struct appdata_t
		{
			float4 vertex   : POSITION;	// 頂点
			float4 color    : COLOR;	// カラー
		};

		// データ処理後の結果を返す構造体
		struct v2f
		{
			float4 vertex   : SV_POSITION;	// 頂点
			fixed4 color : COLOR;			// カラー
		};

		fixed4 _Color;

		// 頂点処理シェーダ
		v2f vert(appdata_t IN)
		{
			v2f OUT;	// ピクセル処理シェーダに渡す構造体宣言
			OUT.vertex = UnityObjectToClipPos(IN.vertex);	// Unityで設定したクリップ設定位置
			OUT.color = IN.color * _Color;	// テクスチャの色設定
#ifdef PIXELSNAP_ON	// ピクセル準拠移動の場合
			OUT.vertex = UnityPixelSnap(OUT.vertex);	// Unityで設定したピクセル設定位置
#endif
			return OUT;
		}

		// ピクセル処理シェーダ
		fixed4 frag(v2f IN) : SV_Target
		{
			fixed4 c = IN.color;	// カラーの設定
		return c;
		}
		ENDCG
		//====== CGプログラム終了 =======//
		}
	}
}
