using System;

namespace Steamworks
{
	// Token: 0x02000267 RID: 615
	public enum EControllerActionOrigin
	{
		// Token: 0x04000897 RID: 2199
		k_EControllerActionOrigin_None,
		// Token: 0x04000898 RID: 2200
		k_EControllerActionOrigin_A,
		// Token: 0x04000899 RID: 2201
		k_EControllerActionOrigin_B,
		// Token: 0x0400089A RID: 2202
		k_EControllerActionOrigin_X,
		// Token: 0x0400089B RID: 2203
		k_EControllerActionOrigin_Y,
		// Token: 0x0400089C RID: 2204
		k_EControllerActionOrigin_LeftBumper,
		// Token: 0x0400089D RID: 2205
		k_EControllerActionOrigin_RightBumper,
		// Token: 0x0400089E RID: 2206
		k_EControllerActionOrigin_LeftGrip,
		// Token: 0x0400089F RID: 2207
		k_EControllerActionOrigin_RightGrip,
		// Token: 0x040008A0 RID: 2208
		k_EControllerActionOrigin_Start,
		// Token: 0x040008A1 RID: 2209
		k_EControllerActionOrigin_Back,
		// Token: 0x040008A2 RID: 2210
		k_EControllerActionOrigin_LeftPad_Touch,
		// Token: 0x040008A3 RID: 2211
		k_EControllerActionOrigin_LeftPad_Swipe,
		// Token: 0x040008A4 RID: 2212
		k_EControllerActionOrigin_LeftPad_Click,
		// Token: 0x040008A5 RID: 2213
		k_EControllerActionOrigin_LeftPad_DPadNorth,
		// Token: 0x040008A6 RID: 2214
		k_EControllerActionOrigin_LeftPad_DPadSouth,
		// Token: 0x040008A7 RID: 2215
		k_EControllerActionOrigin_LeftPad_DPadWest,
		// Token: 0x040008A8 RID: 2216
		k_EControllerActionOrigin_LeftPad_DPadEast,
		// Token: 0x040008A9 RID: 2217
		k_EControllerActionOrigin_RightPad_Touch,
		// Token: 0x040008AA RID: 2218
		k_EControllerActionOrigin_RightPad_Swipe,
		// Token: 0x040008AB RID: 2219
		k_EControllerActionOrigin_RightPad_Click,
		// Token: 0x040008AC RID: 2220
		k_EControllerActionOrigin_RightPad_DPadNorth,
		// Token: 0x040008AD RID: 2221
		k_EControllerActionOrigin_RightPad_DPadSouth,
		// Token: 0x040008AE RID: 2222
		k_EControllerActionOrigin_RightPad_DPadWest,
		// Token: 0x040008AF RID: 2223
		k_EControllerActionOrigin_RightPad_DPadEast,
		// Token: 0x040008B0 RID: 2224
		k_EControllerActionOrigin_LeftTrigger_Pull,
		// Token: 0x040008B1 RID: 2225
		k_EControllerActionOrigin_LeftTrigger_Click,
		// Token: 0x040008B2 RID: 2226
		k_EControllerActionOrigin_RightTrigger_Pull,
		// Token: 0x040008B3 RID: 2227
		k_EControllerActionOrigin_RightTrigger_Click,
		// Token: 0x040008B4 RID: 2228
		k_EControllerActionOrigin_LeftStick_Move,
		// Token: 0x040008B5 RID: 2229
		k_EControllerActionOrigin_LeftStick_Click,
		// Token: 0x040008B6 RID: 2230
		k_EControllerActionOrigin_LeftStick_DPadNorth,
		// Token: 0x040008B7 RID: 2231
		k_EControllerActionOrigin_LeftStick_DPadSouth,
		// Token: 0x040008B8 RID: 2232
		k_EControllerActionOrigin_LeftStick_DPadWest,
		// Token: 0x040008B9 RID: 2233
		k_EControllerActionOrigin_LeftStick_DPadEast,
		// Token: 0x040008BA RID: 2234
		k_EControllerActionOrigin_Gyro_Move,
		// Token: 0x040008BB RID: 2235
		k_EControllerActionOrigin_Gyro_Pitch,
		// Token: 0x040008BC RID: 2236
		k_EControllerActionOrigin_Gyro_Yaw,
		// Token: 0x040008BD RID: 2237
		k_EControllerActionOrigin_Gyro_Roll,
		// Token: 0x040008BE RID: 2238
		k_EControllerActionOrigin_PS4_X,
		// Token: 0x040008BF RID: 2239
		k_EControllerActionOrigin_PS4_Circle,
		// Token: 0x040008C0 RID: 2240
		k_EControllerActionOrigin_PS4_Triangle,
		// Token: 0x040008C1 RID: 2241
		k_EControllerActionOrigin_PS4_Square,
		// Token: 0x040008C2 RID: 2242
		k_EControllerActionOrigin_PS4_LeftBumper,
		// Token: 0x040008C3 RID: 2243
		k_EControllerActionOrigin_PS4_RightBumper,
		// Token: 0x040008C4 RID: 2244
		k_EControllerActionOrigin_PS4_Options,
		// Token: 0x040008C5 RID: 2245
		k_EControllerActionOrigin_PS4_Share,
		// Token: 0x040008C6 RID: 2246
		k_EControllerActionOrigin_PS4_LeftPad_Touch,
		// Token: 0x040008C7 RID: 2247
		k_EControllerActionOrigin_PS4_LeftPad_Swipe,
		// Token: 0x040008C8 RID: 2248
		k_EControllerActionOrigin_PS4_LeftPad_Click,
		// Token: 0x040008C9 RID: 2249
		k_EControllerActionOrigin_PS4_LeftPad_DPadNorth,
		// Token: 0x040008CA RID: 2250
		k_EControllerActionOrigin_PS4_LeftPad_DPadSouth,
		// Token: 0x040008CB RID: 2251
		k_EControllerActionOrigin_PS4_LeftPad_DPadWest,
		// Token: 0x040008CC RID: 2252
		k_EControllerActionOrigin_PS4_LeftPad_DPadEast,
		// Token: 0x040008CD RID: 2253
		k_EControllerActionOrigin_PS4_RightPad_Touch,
		// Token: 0x040008CE RID: 2254
		k_EControllerActionOrigin_PS4_RightPad_Swipe,
		// Token: 0x040008CF RID: 2255
		k_EControllerActionOrigin_PS4_RightPad_Click,
		// Token: 0x040008D0 RID: 2256
		k_EControllerActionOrigin_PS4_RightPad_DPadNorth,
		// Token: 0x040008D1 RID: 2257
		k_EControllerActionOrigin_PS4_RightPad_DPadSouth,
		// Token: 0x040008D2 RID: 2258
		k_EControllerActionOrigin_PS4_RightPad_DPadWest,
		// Token: 0x040008D3 RID: 2259
		k_EControllerActionOrigin_PS4_RightPad_DPadEast,
		// Token: 0x040008D4 RID: 2260
		k_EControllerActionOrigin_PS4_CenterPad_Touch,
		// Token: 0x040008D5 RID: 2261
		k_EControllerActionOrigin_PS4_CenterPad_Swipe,
		// Token: 0x040008D6 RID: 2262
		k_EControllerActionOrigin_PS4_CenterPad_Click,
		// Token: 0x040008D7 RID: 2263
		k_EControllerActionOrigin_PS4_CenterPad_DPadNorth,
		// Token: 0x040008D8 RID: 2264
		k_EControllerActionOrigin_PS4_CenterPad_DPadSouth,
		// Token: 0x040008D9 RID: 2265
		k_EControllerActionOrigin_PS4_CenterPad_DPadWest,
		// Token: 0x040008DA RID: 2266
		k_EControllerActionOrigin_PS4_CenterPad_DPadEast,
		// Token: 0x040008DB RID: 2267
		k_EControllerActionOrigin_PS4_LeftTrigger_Pull,
		// Token: 0x040008DC RID: 2268
		k_EControllerActionOrigin_PS4_LeftTrigger_Click,
		// Token: 0x040008DD RID: 2269
		k_EControllerActionOrigin_PS4_RightTrigger_Pull,
		// Token: 0x040008DE RID: 2270
		k_EControllerActionOrigin_PS4_RightTrigger_Click,
		// Token: 0x040008DF RID: 2271
		k_EControllerActionOrigin_PS4_LeftStick_Move,
		// Token: 0x040008E0 RID: 2272
		k_EControllerActionOrigin_PS4_LeftStick_Click,
		// Token: 0x040008E1 RID: 2273
		k_EControllerActionOrigin_PS4_LeftStick_DPadNorth,
		// Token: 0x040008E2 RID: 2274
		k_EControllerActionOrigin_PS4_LeftStick_DPadSouth,
		// Token: 0x040008E3 RID: 2275
		k_EControllerActionOrigin_PS4_LeftStick_DPadWest,
		// Token: 0x040008E4 RID: 2276
		k_EControllerActionOrigin_PS4_LeftStick_DPadEast,
		// Token: 0x040008E5 RID: 2277
		k_EControllerActionOrigin_PS4_RightStick_Move,
		// Token: 0x040008E6 RID: 2278
		k_EControllerActionOrigin_PS4_RightStick_Click,
		// Token: 0x040008E7 RID: 2279
		k_EControllerActionOrigin_PS4_RightStick_DPadNorth,
		// Token: 0x040008E8 RID: 2280
		k_EControllerActionOrigin_PS4_RightStick_DPadSouth,
		// Token: 0x040008E9 RID: 2281
		k_EControllerActionOrigin_PS4_RightStick_DPadWest,
		// Token: 0x040008EA RID: 2282
		k_EControllerActionOrigin_PS4_RightStick_DPadEast,
		// Token: 0x040008EB RID: 2283
		k_EControllerActionOrigin_PS4_DPad_North,
		// Token: 0x040008EC RID: 2284
		k_EControllerActionOrigin_PS4_DPad_South,
		// Token: 0x040008ED RID: 2285
		k_EControllerActionOrigin_PS4_DPad_West,
		// Token: 0x040008EE RID: 2286
		k_EControllerActionOrigin_PS4_DPad_East,
		// Token: 0x040008EF RID: 2287
		k_EControllerActionOrigin_PS4_Gyro_Move,
		// Token: 0x040008F0 RID: 2288
		k_EControllerActionOrigin_PS4_Gyro_Pitch,
		// Token: 0x040008F1 RID: 2289
		k_EControllerActionOrigin_PS4_Gyro_Yaw,
		// Token: 0x040008F2 RID: 2290
		k_EControllerActionOrigin_PS4_Gyro_Roll,
		// Token: 0x040008F3 RID: 2291
		k_EControllerActionOrigin_XBoxOne_A,
		// Token: 0x040008F4 RID: 2292
		k_EControllerActionOrigin_XBoxOne_B,
		// Token: 0x040008F5 RID: 2293
		k_EControllerActionOrigin_XBoxOne_X,
		// Token: 0x040008F6 RID: 2294
		k_EControllerActionOrigin_XBoxOne_Y,
		// Token: 0x040008F7 RID: 2295
		k_EControllerActionOrigin_XBoxOne_LeftBumper,
		// Token: 0x040008F8 RID: 2296
		k_EControllerActionOrigin_XBoxOne_RightBumper,
		// Token: 0x040008F9 RID: 2297
		k_EControllerActionOrigin_XBoxOne_Menu,
		// Token: 0x040008FA RID: 2298
		k_EControllerActionOrigin_XBoxOne_View,
		// Token: 0x040008FB RID: 2299
		k_EControllerActionOrigin_XBoxOne_LeftTrigger_Pull,
		// Token: 0x040008FC RID: 2300
		k_EControllerActionOrigin_XBoxOne_LeftTrigger_Click,
		// Token: 0x040008FD RID: 2301
		k_EControllerActionOrigin_XBoxOne_RightTrigger_Pull,
		// Token: 0x040008FE RID: 2302
		k_EControllerActionOrigin_XBoxOne_RightTrigger_Click,
		// Token: 0x040008FF RID: 2303
		k_EControllerActionOrigin_XBoxOne_LeftStick_Move,
		// Token: 0x04000900 RID: 2304
		k_EControllerActionOrigin_XBoxOne_LeftStick_Click,
		// Token: 0x04000901 RID: 2305
		k_EControllerActionOrigin_XBoxOne_LeftStick_DPadNorth,
		// Token: 0x04000902 RID: 2306
		k_EControllerActionOrigin_XBoxOne_LeftStick_DPadSouth,
		// Token: 0x04000903 RID: 2307
		k_EControllerActionOrigin_XBoxOne_LeftStick_DPadWest,
		// Token: 0x04000904 RID: 2308
		k_EControllerActionOrigin_XBoxOne_LeftStick_DPadEast,
		// Token: 0x04000905 RID: 2309
		k_EControllerActionOrigin_XBoxOne_RightStick_Move,
		// Token: 0x04000906 RID: 2310
		k_EControllerActionOrigin_XBoxOne_RightStick_Click,
		// Token: 0x04000907 RID: 2311
		k_EControllerActionOrigin_XBoxOne_RightStick_DPadNorth,
		// Token: 0x04000908 RID: 2312
		k_EControllerActionOrigin_XBoxOne_RightStick_DPadSouth,
		// Token: 0x04000909 RID: 2313
		k_EControllerActionOrigin_XBoxOne_RightStick_DPadWest,
		// Token: 0x0400090A RID: 2314
		k_EControllerActionOrigin_XBoxOne_RightStick_DPadEast,
		// Token: 0x0400090B RID: 2315
		k_EControllerActionOrigin_XBoxOne_DPad_North,
		// Token: 0x0400090C RID: 2316
		k_EControllerActionOrigin_XBoxOne_DPad_South,
		// Token: 0x0400090D RID: 2317
		k_EControllerActionOrigin_XBoxOne_DPad_West,
		// Token: 0x0400090E RID: 2318
		k_EControllerActionOrigin_XBoxOne_DPad_East,
		// Token: 0x0400090F RID: 2319
		k_EControllerActionOrigin_XBox360_A,
		// Token: 0x04000910 RID: 2320
		k_EControllerActionOrigin_XBox360_B,
		// Token: 0x04000911 RID: 2321
		k_EControllerActionOrigin_XBox360_X,
		// Token: 0x04000912 RID: 2322
		k_EControllerActionOrigin_XBox360_Y,
		// Token: 0x04000913 RID: 2323
		k_EControllerActionOrigin_XBox360_LeftBumper,
		// Token: 0x04000914 RID: 2324
		k_EControllerActionOrigin_XBox360_RightBumper,
		// Token: 0x04000915 RID: 2325
		k_EControllerActionOrigin_XBox360_Start,
		// Token: 0x04000916 RID: 2326
		k_EControllerActionOrigin_XBox360_Back,
		// Token: 0x04000917 RID: 2327
		k_EControllerActionOrigin_XBox360_LeftTrigger_Pull,
		// Token: 0x04000918 RID: 2328
		k_EControllerActionOrigin_XBox360_LeftTrigger_Click,
		// Token: 0x04000919 RID: 2329
		k_EControllerActionOrigin_XBox360_RightTrigger_Pull,
		// Token: 0x0400091A RID: 2330
		k_EControllerActionOrigin_XBox360_RightTrigger_Click,
		// Token: 0x0400091B RID: 2331
		k_EControllerActionOrigin_XBox360_LeftStick_Move,
		// Token: 0x0400091C RID: 2332
		k_EControllerActionOrigin_XBox360_LeftStick_Click,
		// Token: 0x0400091D RID: 2333
		k_EControllerActionOrigin_XBox360_LeftStick_DPadNorth,
		// Token: 0x0400091E RID: 2334
		k_EControllerActionOrigin_XBox360_LeftStick_DPadSouth,
		// Token: 0x0400091F RID: 2335
		k_EControllerActionOrigin_XBox360_LeftStick_DPadWest,
		// Token: 0x04000920 RID: 2336
		k_EControllerActionOrigin_XBox360_LeftStick_DPadEast,
		// Token: 0x04000921 RID: 2337
		k_EControllerActionOrigin_XBox360_RightStick_Move,
		// Token: 0x04000922 RID: 2338
		k_EControllerActionOrigin_XBox360_RightStick_Click,
		// Token: 0x04000923 RID: 2339
		k_EControllerActionOrigin_XBox360_RightStick_DPadNorth,
		// Token: 0x04000924 RID: 2340
		k_EControllerActionOrigin_XBox360_RightStick_DPadSouth,
		// Token: 0x04000925 RID: 2341
		k_EControllerActionOrigin_XBox360_RightStick_DPadWest,
		// Token: 0x04000926 RID: 2342
		k_EControllerActionOrigin_XBox360_RightStick_DPadEast,
		// Token: 0x04000927 RID: 2343
		k_EControllerActionOrigin_XBox360_DPad_North,
		// Token: 0x04000928 RID: 2344
		k_EControllerActionOrigin_XBox360_DPad_South,
		// Token: 0x04000929 RID: 2345
		k_EControllerActionOrigin_XBox360_DPad_West,
		// Token: 0x0400092A RID: 2346
		k_EControllerActionOrigin_XBox360_DPad_East,
		// Token: 0x0400092B RID: 2347
		k_EControllerActionOrigin_SteamV2_A,
		// Token: 0x0400092C RID: 2348
		k_EControllerActionOrigin_SteamV2_B,
		// Token: 0x0400092D RID: 2349
		k_EControllerActionOrigin_SteamV2_X,
		// Token: 0x0400092E RID: 2350
		k_EControllerActionOrigin_SteamV2_Y,
		// Token: 0x0400092F RID: 2351
		k_EControllerActionOrigin_SteamV2_LeftBumper,
		// Token: 0x04000930 RID: 2352
		k_EControllerActionOrigin_SteamV2_RightBumper,
		// Token: 0x04000931 RID: 2353
		k_EControllerActionOrigin_SteamV2_LeftGrip,
		// Token: 0x04000932 RID: 2354
		k_EControllerActionOrigin_SteamV2_RightGrip,
		// Token: 0x04000933 RID: 2355
		k_EControllerActionOrigin_SteamV2_Start,
		// Token: 0x04000934 RID: 2356
		k_EControllerActionOrigin_SteamV2_Back,
		// Token: 0x04000935 RID: 2357
		k_EControllerActionOrigin_SteamV2_LeftPad_Touch,
		// Token: 0x04000936 RID: 2358
		k_EControllerActionOrigin_SteamV2_LeftPad_Swipe,
		// Token: 0x04000937 RID: 2359
		k_EControllerActionOrigin_SteamV2_LeftPad_Click,
		// Token: 0x04000938 RID: 2360
		k_EControllerActionOrigin_SteamV2_LeftPad_DPadNorth,
		// Token: 0x04000939 RID: 2361
		k_EControllerActionOrigin_SteamV2_LeftPad_DPadSouth,
		// Token: 0x0400093A RID: 2362
		k_EControllerActionOrigin_SteamV2_LeftPad_DPadWest,
		// Token: 0x0400093B RID: 2363
		k_EControllerActionOrigin_SteamV2_LeftPad_DPadEast,
		// Token: 0x0400093C RID: 2364
		k_EControllerActionOrigin_SteamV2_RightPad_Touch,
		// Token: 0x0400093D RID: 2365
		k_EControllerActionOrigin_SteamV2_RightPad_Swipe,
		// Token: 0x0400093E RID: 2366
		k_EControllerActionOrigin_SteamV2_RightPad_Click,
		// Token: 0x0400093F RID: 2367
		k_EControllerActionOrigin_SteamV2_RightPad_DPadNorth,
		// Token: 0x04000940 RID: 2368
		k_EControllerActionOrigin_SteamV2_RightPad_DPadSouth,
		// Token: 0x04000941 RID: 2369
		k_EControllerActionOrigin_SteamV2_RightPad_DPadWest,
		// Token: 0x04000942 RID: 2370
		k_EControllerActionOrigin_SteamV2_RightPad_DPadEast,
		// Token: 0x04000943 RID: 2371
		k_EControllerActionOrigin_SteamV2_LeftTrigger_Pull,
		// Token: 0x04000944 RID: 2372
		k_EControllerActionOrigin_SteamV2_LeftTrigger_Click,
		// Token: 0x04000945 RID: 2373
		k_EControllerActionOrigin_SteamV2_RightTrigger_Pull,
		// Token: 0x04000946 RID: 2374
		k_EControllerActionOrigin_SteamV2_RightTrigger_Click,
		// Token: 0x04000947 RID: 2375
		k_EControllerActionOrigin_SteamV2_LeftStick_Move,
		// Token: 0x04000948 RID: 2376
		k_EControllerActionOrigin_SteamV2_LeftStick_Click,
		// Token: 0x04000949 RID: 2377
		k_EControllerActionOrigin_SteamV2_LeftStick_DPadNorth,
		// Token: 0x0400094A RID: 2378
		k_EControllerActionOrigin_SteamV2_LeftStick_DPadSouth,
		// Token: 0x0400094B RID: 2379
		k_EControllerActionOrigin_SteamV2_LeftStick_DPadWest,
		// Token: 0x0400094C RID: 2380
		k_EControllerActionOrigin_SteamV2_LeftStick_DPadEast,
		// Token: 0x0400094D RID: 2381
		k_EControllerActionOrigin_SteamV2_Gyro_Move,
		// Token: 0x0400094E RID: 2382
		k_EControllerActionOrigin_SteamV2_Gyro_Pitch,
		// Token: 0x0400094F RID: 2383
		k_EControllerActionOrigin_SteamV2_Gyro_Yaw,
		// Token: 0x04000950 RID: 2384
		k_EControllerActionOrigin_SteamV2_Gyro_Roll,
		// Token: 0x04000951 RID: 2385
		k_EControllerActionOrigin_Count
	}
}
