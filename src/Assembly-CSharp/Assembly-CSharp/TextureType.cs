using System;

// Token: 0x0200018F RID: 399
public enum TextureType
{
	// Token: 0x04000C61 RID: 3169
	None,
	// Token: 0x04000C62 RID: 3170
	Gradient = 2,
	// Token: 0x04000C63 RID: 3171
	Geometry_Gradient = 161,
	// Token: 0x04000C64 RID: 3172
	WoodGrain = 3,
	// Token: 0x04000C65 RID: 3173
	VoronoiPolys,
	// Token: 0x04000C66 RID: 3174
	WavyLines,
	// Token: 0x04000C67 RID: 3175
	VoronoiDots,
	// Token: 0x04000C68 RID: 3176
	PerlinNoise1,
	// Token: 0x04000C69 RID: 3177
	QuasiCrystal,
	// Token: 0x04000C6A RID: 3178
	PlasmaNoise = 12,
	// Token: 0x04000C6B RID: 3179
	Pool,
	// Token: 0x04000C6C RID: 3180
	Bio,
	// Token: 0x04000C6D RID: 3181
	FractalNoise,
	// Token: 0x04000C6E RID: 3182
	LightSquares,
	// Token: 0x04000C6F RID: 3183
	Machine,
	// Token: 0x04000C70 RID: 3184
	SweptNoise,
	// Token: 0x04000C71 RID: 3185
	Abstract,
	// Token: 0x04000C72 RID: 3186
	Dashes,
	// Token: 0x04000C73 RID: 3187
	LayeredNoise,
	// Token: 0x04000C74 RID: 3188
	SquareRegress,
	// Token: 0x04000C75 RID: 3189
	Swirly,
	// Token: 0x04000C76 RID: 3190
	SideGlow = 10,
	// Token: 0x04000C77 RID: 3191
	Ground_Spotty = 24,
	// Token: 0x04000C78 RID: 3192
	Ground_SpottyBumpMap,
	// Token: 0x04000C79 RID: 3193
	Ground_LineBumpMap,
	// Token: 0x04000C7A RID: 3194
	Ground_SplitBumpMap,
	// Token: 0x04000C7B RID: 3195
	Ground_Wet,
	// Token: 0x04000C7C RID: 3196
	Ground_Rocky,
	// Token: 0x04000C7D RID: 3197
	Ground_RockyBumpmap,
	// Token: 0x04000C7E RID: 3198
	Ground_Broken,
	// Token: 0x04000C7F RID: 3199
	Ground_BrokenBumpMap,
	// Token: 0x04000C80 RID: 3200
	Ground_Pebbles,
	// Token: 0x04000C81 RID: 3201
	Ground_PebblesBumpMap,
	// Token: 0x04000C82 RID: 3202
	Ground_1BumpMap,
	// Token: 0x04000C83 RID: 3203
	Misc_IceSoft = 11,
	// Token: 0x04000C84 RID: 3204
	Misc_CrackedIce = 36,
	// Token: 0x04000C85 RID: 3205
	Misc_CrackedGround,
	// Token: 0x04000C86 RID: 3206
	Misc_LinesPattern,
	// Token: 0x04000C87 RID: 3207
	Misc_StoneGround,
	// Token: 0x04000C88 RID: 3208
	Misc_Lava,
	// Token: 0x04000C89 RID: 3209
	Misc_LavaBumpMap,
	// Token: 0x04000C8A RID: 3210
	Misc_StraightIce,
	// Token: 0x04000C8B RID: 3211
	Misc_CrackedIce2,
	// Token: 0x04000C8C RID: 3212
	Misc_Shades,
	// Token: 0x04000C8D RID: 3213
	Misc_Cork,
	// Token: 0x04000C8E RID: 3214
	Misc_Wool,
	// Token: 0x04000C8F RID: 3215
	Misc_Salad,
	// Token: 0x04000C90 RID: 3216
	Misc_CrossLines,
	// Token: 0x04000C91 RID: 3217
	Misc_Holes,
	// Token: 0x04000C92 RID: 3218
	Misc_Waves,
	// Token: 0x04000C93 RID: 3219
	Misc_WavesBumpMap,
	// Token: 0x04000C94 RID: 3220
	Misc_1 = 155,
	// Token: 0x04000C95 RID: 3221
	Misc_1BumpMap = 52,
	// Token: 0x04000C96 RID: 3222
	Misc_2,
	// Token: 0x04000C97 RID: 3223
	Misc_2BumpMap,
	// Token: 0x04000C98 RID: 3224
	Misc_3,
	// Token: 0x04000C99 RID: 3225
	Misc_3BumpMap,
	// Token: 0x04000C9A RID: 3226
	Misc_SoftNoise,
	// Token: 0x04000C9B RID: 3227
	Misc_SoftNoiseBumpMap,
	// Token: 0x04000C9C RID: 3228
	Misc_SoftNoiseBumpMapVariant = 157,
	// Token: 0x04000C9D RID: 3229
	Misc_Stars = 59,
	// Token: 0x04000C9E RID: 3230
	Misc_StarsBumpMap,
	// Token: 0x04000C9F RID: 3231
	Misc_CottonBalls,
	// Token: 0x04000CA0 RID: 3232
	Misc_4,
	// Token: 0x04000CA1 RID: 3233
	Misc_4BumpMap,
	// Token: 0x04000CA2 RID: 3234
	Misc_5,
	// Token: 0x04000CA3 RID: 3235
	Misc_5BumpMap,
	// Token: 0x04000CA4 RID: 3236
	Misc_Glass,
	// Token: 0x04000CA5 RID: 3237
	Geometry_Circle_1,
	// Token: 0x04000CA6 RID: 3238
	Geometry_Circle_2,
	// Token: 0x04000CA7 RID: 3239
	Geometry_Circle_3,
	// Token: 0x04000CA8 RID: 3240
	Geometry_Half,
	// Token: 0x04000CA9 RID: 3241
	Geometry_TiltedHalf,
	// Token: 0x04000CAA RID: 3242
	Geometry_Pyramid,
	// Token: 0x04000CAB RID: 3243
	Geometry_Dots,
	// Token: 0x04000CAC RID: 3244
	Geometry_MultiGradient,
	// Token: 0x04000CAD RID: 3245
	Geometry_Wave,
	// Token: 0x04000CAE RID: 3246
	Geometry_Checkerboard,
	// Token: 0x04000CAF RID: 3247
	Geometry_CheckerboardBumpMap,
	// Token: 0x04000CB0 RID: 3248
	Geometry_Lines,
	// Token: 0x04000CB1 RID: 3249
	Geometry_LinesBumpMap,
	// Token: 0x04000CB2 RID: 3250
	Geometry_DoubleGradient,
	// Token: 0x04000CB3 RID: 3251
	Geometry_Rectangles,
	// Token: 0x04000CB4 RID: 3252
	Geometry_RectanglesBumpMap,
	// Token: 0x04000CB5 RID: 3253
	Geometry_Border_1 = 156,
	// Token: 0x04000CB6 RID: 3254
	Geometry_Border_2 = 83,
	// Token: 0x04000CB7 RID: 3255
	Geometry_Border_3,
	// Token: 0x04000CB8 RID: 3256
	Geometry_Border_4,
	// Token: 0x04000CB9 RID: 3257
	Geometry_Border_2BumpMap,
	// Token: 0x04000CBA RID: 3258
	Geometry_Circle_2BumpMap,
	// Token: 0x04000CBB RID: 3259
	Geometry_Lines2,
	// Token: 0x04000CBC RID: 3260
	Geometry_Lines2Blurred,
	// Token: 0x04000CBD RID: 3261
	Geometry_RoundBorder,
	// Token: 0x04000CBE RID: 3262
	Geometry_RoundBorder2,
	// Token: 0x04000CBF RID: 3263
	Geometry_RoundBorderBumpMap,
	// Token: 0x04000CC0 RID: 3264
	Geometry_Line_1,
	// Token: 0x04000CC1 RID: 3265
	Geometry_Line_2,
	// Token: 0x04000CC2 RID: 3266
	Geometry_Line_3,
	// Token: 0x04000CC3 RID: 3267
	Geometry_Line_2BumpMap,
	// Token: 0x04000CC4 RID: 3268
	Geometry_Octagon_1,
	// Token: 0x04000CC5 RID: 3269
	Geometry_Octagon_2,
	// Token: 0x04000CC6 RID: 3270
	Geometry_Octagon_3,
	// Token: 0x04000CC7 RID: 3271
	Geometry_Hexagon,
	// Token: 0x04000CC8 RID: 3272
	Geometry_Hexagon2,
	// Token: 0x04000CC9 RID: 3273
	Geometry_Hexagon2BumpMap,
	// Token: 0x04000CCA RID: 3274
	Metal_1,
	// Token: 0x04000CCB RID: 3275
	Metal_2,
	// Token: 0x04000CCC RID: 3276
	Metal_Wet,
	// Token: 0x04000CCD RID: 3277
	Marble_1,
	// Token: 0x04000CCE RID: 3278
	Marble_2,
	// Token: 0x04000CCF RID: 3279
	Marble_3,
	// Token: 0x04000CD0 RID: 3280
	Tree_1BumpMap,
	// Token: 0x04000CD1 RID: 3281
	Tree_2,
	// Token: 0x04000CD2 RID: 3282
	Tree_3,
	// Token: 0x04000CD3 RID: 3283
	Tree_4,
	// Token: 0x04000CD4 RID: 3284
	Grass_4,
	// Token: 0x04000CD5 RID: 3285
	Grass_4BumpMap,
	// Token: 0x04000CD6 RID: 3286
	Grass_3,
	// Token: 0x04000CD7 RID: 3287
	Grass_3BumpMap,
	// Token: 0x04000CD8 RID: 3288
	Grass_2,
	// Token: 0x04000CD9 RID: 3289
	Grass_1 = 119,
	// Token: 0x04000CDA RID: 3290
	Grass_1BumpMap,
	// Token: 0x04000CDB RID: 3291
	Grass_2BumpMap = 118,
	// Token: 0x04000CDC RID: 3292
	Grass_5 = 121,
	// Token: 0x04000CDD RID: 3293
	Grass_5BumpMap,
	// Token: 0x04000CDE RID: 3294
	Grass_6BumpMap,
	// Token: 0x04000CDF RID: 3295
	Wall_1,
	// Token: 0x04000CE0 RID: 3296
	Wall_1BumpMap,
	// Token: 0x04000CE1 RID: 3297
	Wall_2,
	// Token: 0x04000CE2 RID: 3298
	Wall_2BumpMap,
	// Token: 0x04000CE3 RID: 3299
	Wall_3BumpMap,
	// Token: 0x04000CE4 RID: 3300
	Wall_Rocky,
	// Token: 0x04000CE5 RID: 3301
	Wall_RockyBumpMap,
	// Token: 0x04000CE6 RID: 3302
	Wall_Freckles,
	// Token: 0x04000CE7 RID: 3303
	Wall_FrecklesBumpMap,
	// Token: 0x04000CE8 RID: 3304
	Wall_Freckles2,
	// Token: 0x04000CE9 RID: 3305
	Wall_Freckles2BumpMap,
	// Token: 0x04000CEA RID: 3306
	Wall_Scratches,
	// Token: 0x04000CEB RID: 3307
	Wall_ScratchesBumpMap,
	// Token: 0x04000CEC RID: 3308
	Wall_Mossy,
	// Token: 0x04000CED RID: 3309
	Wall_MossyBumpMap,
	// Token: 0x04000CEE RID: 3310
	Wall_Wavy,
	// Token: 0x04000CEF RID: 3311
	Wall_WavyBumpMap,
	// Token: 0x04000CF0 RID: 3312
	Wall_Lines,
	// Token: 0x04000CF1 RID: 3313
	Wall_LinesBumpMap,
	// Token: 0x04000CF2 RID: 3314
	Wall_Lines2,
	// Token: 0x04000CF3 RID: 3315
	Wall_Lines2BumpMap,
	// Token: 0x04000CF4 RID: 3316
	Wall_Lines3,
	// Token: 0x04000CF5 RID: 3317
	Wall_Lines3BumpMap,
	// Token: 0x04000CF6 RID: 3318
	Wall_ScratchyLines,
	// Token: 0x04000CF7 RID: 3319
	Wall_ScratchyLinesBumpMap,
	// Token: 0x04000CF8 RID: 3320
	Cloth_1,
	// Token: 0x04000CF9 RID: 3321
	Cloth_2,
	// Token: 0x04000CFA RID: 3322
	Cloth_2BumpMap,
	// Token: 0x04000CFB RID: 3323
	Cloth_3,
	// Token: 0x04000CFC RID: 3324
	Cloth_4,
	// Token: 0x04000CFD RID: 3325
	Cloth_4BumpMap,
	// Token: 0x04000CFE RID: 3326
	Geometry_RadialGradient = 158,
	// Token: 0x04000CFF RID: 3327
	Geometry_MoreLines,
	// Token: 0x04000D00 RID: 3328
	Geometry_MoreRectangles,
	// Token: 0x04000D01 RID: 3329
	Filled = 162,
	// Token: 0x04000D02 RID: 3330
	Vertex_Scatter,
	// Token: 0x04000D03 RID: 3331
	Vertex_Expand = 165,
	// Token: 0x04000D04 RID: 3332
	Vertex_Slice,
	// Token: 0x04000D05 RID: 3333
	Wireframe,
	// Token: 0x04000D06 RID: 3334
	Outline
}
