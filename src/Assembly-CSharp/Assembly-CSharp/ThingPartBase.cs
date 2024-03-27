using System;

// Token: 0x02000192 RID: 402
public enum ThingPartBase
{
	// Token: 0x04000D69 RID: 3433
	Cube = 1,
	// Token: 0x04000D6A RID: 3434
	Pyramid,
	// Token: 0x04000D6B RID: 3435
	Sphere,
	// Token: 0x04000D6C RID: 3436
	Cone,
	// Token: 0x04000D6D RID: 3437
	Cylinder,
	// Token: 0x04000D6E RID: 3438
	Triangle,
	// Token: 0x04000D6F RID: 3439
	Trapeze,
	// Token: 0x04000D70 RID: 3440
	Hedra,
	// Token: 0x04000D71 RID: 3441
	Icosphere,
	// Token: 0x04000D72 RID: 3442
	LowPolySphere,
	// Token: 0x04000D73 RID: 3443
	Ramp,
	// Token: 0x04000D74 RID: 3444
	JitterCube,
	// Token: 0x04000D75 RID: 3445
	ChamferCube,
	// Token: 0x04000D76 RID: 3446
	Spike,
	// Token: 0x04000D77 RID: 3447
	LowPolyCylinder,
	// Token: 0x04000D78 RID: 3448
	HalfSphere,
	// Token: 0x04000D79 RID: 3449
	JitterSphere,
	// Token: 0x04000D7A RID: 3450
	TextArialBold,
	// Token: 0x04000D7B RID: 3451
	TextTimesBold,
	// Token: 0x04000D7C RID: 3452
	TextArCena,
	// Token: 0x04000D7D RID: 3453
	TextGeometr,
	// Token: 0x04000D7E RID: 3454
	TextRockwell,
	// Token: 0x04000D7F RID: 3455
	TextGillSans,
	// Token: 0x04000D80 RID: 3456
	BigDialog = 25,
	// Token: 0x04000D81 RID: 3457
	QuarterPipe1,
	// Token: 0x04000D82 RID: 3458
	QuarterPipe2,
	// Token: 0x04000D83 RID: 3459
	QuarterPipe3,
	// Token: 0x04000D84 RID: 3460
	QuarterPipe4,
	// Token: 0x04000D85 RID: 3461
	QuarterPipe5,
	// Token: 0x04000D86 RID: 3462
	QuarterPipe6,
	// Token: 0x04000D87 RID: 3463
	QuarterTorus1,
	// Token: 0x04000D88 RID: 3464
	QuarterTorus2,
	// Token: 0x04000D89 RID: 3465
	QuarterTorus3,
	// Token: 0x04000D8A RID: 3466
	QuarterTorus4,
	// Token: 0x04000D8B RID: 3467
	QuarterTorus5,
	// Token: 0x04000D8C RID: 3468
	QuarterTorus6,
	// Token: 0x04000D8D RID: 3469
	CurvedRamp,
	// Token: 0x04000D8E RID: 3470
	CubeRotated,
	// Token: 0x04000D8F RID: 3471
	QuarterPipeRotated1,
	// Token: 0x04000D90 RID: 3472
	QuarterPipeRotated2,
	// Token: 0x04000D91 RID: 3473
	QuarterPipeRotated3,
	// Token: 0x04000D92 RID: 3474
	QuarterPipeRotated4,
	// Token: 0x04000D93 RID: 3475
	QuarterPipeRotated5,
	// Token: 0x04000D94 RID: 3476
	QuarterPipeRotated6,
	// Token: 0x04000D95 RID: 3477
	QuarterTorusRotated1,
	// Token: 0x04000D96 RID: 3478
	QuarterTorusRotated2,
	// Token: 0x04000D97 RID: 3479
	QuarterTorusRotated3,
	// Token: 0x04000D98 RID: 3480
	QuarterTorusRotated4,
	// Token: 0x04000D99 RID: 3481
	QuarterTorusRotated5,
	// Token: 0x04000D9A RID: 3482
	QuarterTorusRotated6,
	// Token: 0x04000D9B RID: 3483
	Hexagon,
	// Token: 0x04000D9C RID: 3484
	HexagonBevel,
	// Token: 0x04000D9D RID: 3485
	Ring1,
	// Token: 0x04000D9E RID: 3486
	Ring2,
	// Token: 0x04000D9F RID: 3487
	Ring3,
	// Token: 0x04000DA0 RID: 3488
	Ring4,
	// Token: 0x04000DA1 RID: 3489
	Ring5,
	// Token: 0x04000DA2 RID: 3490
	Ring6,
	// Token: 0x04000DA3 RID: 3491
	CubeBevel1,
	// Token: 0x04000DA4 RID: 3492
	CubeBevel2,
	// Token: 0x04000DA5 RID: 3493
	CubeBevel3,
	// Token: 0x04000DA6 RID: 3494
	Triangle2,
	// Token: 0x04000DA7 RID: 3495
	HalfCylinder,
	// Token: 0x04000DA8 RID: 3496
	QuarterCylinder,
	// Token: 0x04000DA9 RID: 3497
	QuarterSphere,
	// Token: 0x04000DAA RID: 3498
	SphereEdge,
	// Token: 0x04000DAB RID: 3499
	RoundCube,
	// Token: 0x04000DAC RID: 3500
	Capsule,
	// Token: 0x04000DAD RID: 3501
	Heptagon,
	// Token: 0x04000DAE RID: 3502
	Pentagon,
	// Token: 0x04000DAF RID: 3503
	Octagon,
	// Token: 0x04000DB0 RID: 3504
	HighPolySphere,
	// Token: 0x04000DB1 RID: 3505
	Bowl1,
	// Token: 0x04000DB2 RID: 3506
	Bowl2,
	// Token: 0x04000DB3 RID: 3507
	Bowl3,
	// Token: 0x04000DB4 RID: 3508
	Bowl4,
	// Token: 0x04000DB5 RID: 3509
	Bowl5,
	// Token: 0x04000DB6 RID: 3510
	Bowl6,
	// Token: 0x04000DB7 RID: 3511
	BowlCube,
	// Token: 0x04000DB8 RID: 3512
	QuarterBowlCube,
	// Token: 0x04000DB9 RID: 3513
	CubeHole,
	// Token: 0x04000DBA RID: 3514
	HalfCubeHole,
	// Token: 0x04000DBB RID: 3515
	BowlCubeSoft,
	// Token: 0x04000DBC RID: 3516
	QuarterBowlCubeSoft,
	// Token: 0x04000DBD RID: 3517
	Bowl1Soft,
	// Token: 0x04000DBE RID: 3518
	QuarterSphereRotated,
	// Token: 0x04000DBF RID: 3519
	HalfBowlSoft,
	// Token: 0x04000DC0 RID: 3520
	QuarterBowlSoft,
	// Token: 0x04000DC1 RID: 3521
	TextOrbitron,
	// Token: 0x04000DC2 RID: 3522
	TextAlfaSlab,
	// Token: 0x04000DC3 RID: 3523
	TextAudioWide,
	// Token: 0x04000DC4 RID: 3524
	TextBangers,
	// Token: 0x04000DC5 RID: 3525
	TextBowlby,
	// Token: 0x04000DC6 RID: 3526
	TextCantata,
	// Token: 0x04000DC7 RID: 3527
	TextChewy,
	// Token: 0x04000DC8 RID: 3528
	TextComingSoon,
	// Token: 0x04000DC9 RID: 3529
	TextExo,
	// Token: 0x04000DCA RID: 3530
	TextFredoka,
	// Token: 0x04000DCB RID: 3531
	TextKaushan,
	// Token: 0x04000DCC RID: 3532
	TextMonoton,
	// Token: 0x04000DCD RID: 3533
	TextPatrick,
	// Token: 0x04000DCE RID: 3534
	TextPirata,
	// Token: 0x04000DCF RID: 3535
	TextShrikhand,
	// Token: 0x04000DD0 RID: 3536
	TextSpinnaker,
	// Token: 0x04000DD1 RID: 3537
	TextDiplomata,
	// Token: 0x04000DD2 RID: 3538
	TextHanalei,
	// Token: 0x04000DD3 RID: 3539
	TextJoti,
	// Token: 0x04000DD4 RID: 3540
	TextMedieval,
	// Token: 0x04000DD5 RID: 3541
	TextSancreek,
	// Token: 0x04000DD6 RID: 3542
	TextStalinist,
	// Token: 0x04000DD7 RID: 3543
	TextTradewinds,
	// Token: 0x04000DD8 RID: 3544
	TextBaloo,
	// Token: 0x04000DD9 RID: 3545
	TextBubbler,
	// Token: 0x04000DDA RID: 3546
	TextCaesarDressing,
	// Token: 0x04000DDB RID: 3547
	TextDhurjati,
	// Token: 0x04000DDC RID: 3548
	TextFascinateInline,
	// Token: 0x04000DDD RID: 3549
	TextFelipa,
	// Token: 0x04000DDE RID: 3550
	TextInknutAntiqua,
	// Token: 0x04000DDF RID: 3551
	TextKeania,
	// Token: 0x04000DE0 RID: 3552
	TextLakkiReddy,
	// Token: 0x04000DE1 RID: 3553
	TextLondrinaSketch,
	// Token: 0x04000DE2 RID: 3554
	TextLondrinaSolid,
	// Token: 0x04000DE3 RID: 3555
	TextMetalMania,
	// Token: 0x04000DE4 RID: 3556
	TextMolle,
	// Token: 0x04000DE5 RID: 3557
	TextRisque,
	// Token: 0x04000DE6 RID: 3558
	TextSarpanch,
	// Token: 0x04000DE7 RID: 3559
	TextUncialAntiqua,
	// Token: 0x04000DE8 RID: 3560
	Text256Bytes,
	// Token: 0x04000DE9 RID: 3561
	TextAeroviasBrasil,
	// Token: 0x04000DEA RID: 3562
	TextAnitaSemiSquare,
	// Token: 0x04000DEB RID: 3563
	TextBeefd,
	// Token: 0x04000DEC RID: 3564
	TextCrackdown,
	// Token: 0x04000DED RID: 3565
	TextCreampuff,
	// Token: 0x04000DEE RID: 3566
	TextDataControl,
	// Token: 0x04000DEF RID: 3567
	TextEndor,
	// Token: 0x04000DF0 RID: 3568
	TextFreshMarker,
	// Token: 0x04000DF1 RID: 3569
	TextHeavyData,
	// Token: 0x04000DF2 RID: 3570
	TextPropaganda,
	// Token: 0x04000DF3 RID: 3571
	TextRobotoMono,
	// Token: 0x04000DF4 RID: 3572
	TextTitania,
	// Token: 0x04000DF5 RID: 3573
	TextTobagoPoster,
	// Token: 0x04000DF6 RID: 3574
	TextViafont,
	// Token: 0x04000DF7 RID: 3575
	Wheel,
	// Token: 0x04000DF8 RID: 3576
	Wheel2,
	// Token: 0x04000DF9 RID: 3577
	Wheel3,
	// Token: 0x04000DFA RID: 3578
	Wheel4,
	// Token: 0x04000DFB RID: 3579
	WheelVariant,
	// Token: 0x04000DFC RID: 3580
	Wheel2Variant,
	// Token: 0x04000DFD RID: 3581
	JitterCubeSoft,
	// Token: 0x04000DFE RID: 3582
	JitterSphereSoft,
	// Token: 0x04000DFF RID: 3583
	LowJitterCube,
	// Token: 0x04000E00 RID: 3584
	LowJitterCubeSoft,
	// Token: 0x04000E01 RID: 3585
	JitterCone,
	// Token: 0x04000E02 RID: 3586
	JitterConeSoft,
	// Token: 0x04000E03 RID: 3587
	JitterHalfCone,
	// Token: 0x04000E04 RID: 3588
	JitterHalfConeSoft,
	// Token: 0x04000E05 RID: 3589
	JitterChamferCylinder,
	// Token: 0x04000E06 RID: 3590
	JitterChamferCylinderSoft,
	// Token: 0x04000E07 RID: 3591
	Gear,
	// Token: 0x04000E08 RID: 3592
	GearVariant,
	// Token: 0x04000E09 RID: 3593
	GearVariant2,
	// Token: 0x04000E0A RID: 3594
	GearSoft,
	// Token: 0x04000E0B RID: 3595
	GearVariantSoft,
	// Token: 0x04000E0C RID: 3596
	GearVariant2Soft,
	// Token: 0x04000E0D RID: 3597
	Branch,
	// Token: 0x04000E0E RID: 3598
	Bubbles,
	// Token: 0x04000E0F RID: 3599
	HoleWall,
	// Token: 0x04000E10 RID: 3600
	JaggyWall,
	// Token: 0x04000E11 RID: 3601
	Rocky,
	// Token: 0x04000E12 RID: 3602
	RockySoft,
	// Token: 0x04000E13 RID: 3603
	RockyVerySoft,
	// Token: 0x04000E14 RID: 3604
	Spikes,
	// Token: 0x04000E15 RID: 3605
	SpikesSoft,
	// Token: 0x04000E16 RID: 3606
	SpikesVerySoft,
	// Token: 0x04000E17 RID: 3607
	WavyWall,
	// Token: 0x04000E18 RID: 3608
	WavyWallVariant,
	// Token: 0x04000E19 RID: 3609
	WavyWallVariantSoft,
	// Token: 0x04000E1A RID: 3610
	Quad,
	// Token: 0x04000E1B RID: 3611
	FineSphere,
	// Token: 0x04000E1C RID: 3612
	Drop,
	// Token: 0x04000E1D RID: 3613
	Drop2,
	// Token: 0x04000E1E RID: 3614
	Drop3,
	// Token: 0x04000E1F RID: 3615
	DropSharp,
	// Token: 0x04000E20 RID: 3616
	DropSharp2,
	// Token: 0x04000E21 RID: 3617
	DropSharp3,
	// Token: 0x04000E22 RID: 3618
	Drop3Flat,
	// Token: 0x04000E23 RID: 3619
	DropSharp3Flat,
	// Token: 0x04000E24 RID: 3620
	DropCut,
	// Token: 0x04000E25 RID: 3621
	DropSharpCut,
	// Token: 0x04000E26 RID: 3622
	DropRing,
	// Token: 0x04000E27 RID: 3623
	DropRingFlat,
	// Token: 0x04000E28 RID: 3624
	DropPear,
	// Token: 0x04000E29 RID: 3625
	DropPear2,
	// Token: 0x04000E2A RID: 3626
	Drop3Jitter,
	// Token: 0x04000E2B RID: 3627
	DropBent,
	// Token: 0x04000E2C RID: 3628
	DropBent2,
	// Token: 0x04000E2D RID: 3629
	SharpBent,
	// Token: 0x04000E2E RID: 3630
	Tetrahedron,
	// Token: 0x04000E2F RID: 3631
	Pipe,
	// Token: 0x04000E30 RID: 3632
	Pipe2,
	// Token: 0x04000E31 RID: 3633
	Pipe3,
	// Token: 0x04000E32 RID: 3634
	ShrinkDisk,
	// Token: 0x04000E33 RID: 3635
	ShrinkDisk2,
	// Token: 0x04000E34 RID: 3636
	DirectionIndicator,
	// Token: 0x04000E35 RID: 3637
	Cube3x2 = 207,
	// Token: 0x04000E36 RID: 3638
	Cube4x2,
	// Token: 0x04000E37 RID: 3639
	Cube5x2,
	// Token: 0x04000E38 RID: 3640
	Cube6x2,
	// Token: 0x04000E39 RID: 3641
	Cube2x3,
	// Token: 0x04000E3A RID: 3642
	Cube3x3,
	// Token: 0x04000E3B RID: 3643
	Cube4x3,
	// Token: 0x04000E3C RID: 3644
	Cube5x3,
	// Token: 0x04000E3D RID: 3645
	Cube6x3,
	// Token: 0x04000E3E RID: 3646
	Cube2x4,
	// Token: 0x04000E3F RID: 3647
	Cube3x4,
	// Token: 0x04000E40 RID: 3648
	Cube4x4,
	// Token: 0x04000E41 RID: 3649
	Cube5x4,
	// Token: 0x04000E42 RID: 3650
	Cube6x4,
	// Token: 0x04000E43 RID: 3651
	Cube2x5,
	// Token: 0x04000E44 RID: 3652
	Cube3x5,
	// Token: 0x04000E45 RID: 3653
	Cube4x5,
	// Token: 0x04000E46 RID: 3654
	Cube5x6deprecated,
	// Token: 0x04000E47 RID: 3655
	Cube5x5 = 252,
	// Token: 0x04000E48 RID: 3656
	Cube6x5 = 225,
	// Token: 0x04000E49 RID: 3657
	Cube6x6,
	// Token: 0x04000E4A RID: 3658
	Quad3x2,
	// Token: 0x04000E4B RID: 3659
	Quad4x2,
	// Token: 0x04000E4C RID: 3660
	Quad5x2,
	// Token: 0x04000E4D RID: 3661
	Quad6x2,
	// Token: 0x04000E4E RID: 3662
	Quad2x3,
	// Token: 0x04000E4F RID: 3663
	Quad3x3,
	// Token: 0x04000E50 RID: 3664
	Quad4x3,
	// Token: 0x04000E51 RID: 3665
	Quad5x3,
	// Token: 0x04000E52 RID: 3666
	Quad6x3,
	// Token: 0x04000E53 RID: 3667
	Quad2x4,
	// Token: 0x04000E54 RID: 3668
	Quad3x4,
	// Token: 0x04000E55 RID: 3669
	Quad4x4,
	// Token: 0x04000E56 RID: 3670
	Quad5x4,
	// Token: 0x04000E57 RID: 3671
	Quad6x4,
	// Token: 0x04000E58 RID: 3672
	Quad2x5,
	// Token: 0x04000E59 RID: 3673
	Quad3x5,
	// Token: 0x04000E5A RID: 3674
	Quad4x5,
	// Token: 0x04000E5B RID: 3675
	Quad5x5,
	// Token: 0x04000E5C RID: 3676
	Quad6x5,
	// Token: 0x04000E5D RID: 3677
	Quad6x6,
	// Token: 0x04000E5E RID: 3678
	Icosahedron,
	// Token: 0x04000E5F RID: 3679
	Cubeoctahedron,
	// Token: 0x04000E60 RID: 3680
	Dodecahedron,
	// Token: 0x04000E61 RID: 3681
	Icosidodecahedron,
	// Token: 0x04000E62 RID: 3682
	Octahedron
}
