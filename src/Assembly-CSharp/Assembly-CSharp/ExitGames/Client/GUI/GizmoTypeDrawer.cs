using System;
using UnityEngine;

namespace ExitGames.Client.GUI
{
	// Token: 0x02000060 RID: 96
	public class GizmoTypeDrawer
	{
		// Token: 0x06000322 RID: 802 RVA: 0x0000C9E8 File Offset: 0x0000ADE8
		public static void Draw(Vector3 center, GizmoType type, Color color, float size)
		{
			Gizmos.color = color;
			switch (type)
			{
			case GizmoType.WireSphere:
				Gizmos.DrawWireSphere(center, size * 0.5f);
				break;
			case GizmoType.Sphere:
				Gizmos.DrawSphere(center, size * 0.5f);
				break;
			case GizmoType.WireCube:
				Gizmos.DrawWireCube(center, Vector3.one * size);
				break;
			case GizmoType.Cube:
				Gizmos.DrawCube(center, Vector3.one * size);
				break;
			}
		}
	}
}
