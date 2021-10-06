using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSpawner : Spawner
{
    public enum CameraRelativityModifier {
        LEFT,
        ABOVE,
        RIGHT,
        BELOW,
        HORIZONTALMIDDLE,
        VERTICALMIDDLE
    }
    public List<CameraRelativityModifier> position = new List<CameraRelativityModifier>();
    public Vector2 offset = new Vector2(0,0);

    private Vector2 GetSpawnPoint(){
        Vector3 middle = Camera.main.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0));
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));

        Vector2 pos = new Vector2(0,0);
        if (position.Contains(CameraRelativityModifier.LEFT)){
            pos.x = bottomLeft.x;
        }
        if (position.Contains(CameraRelativityModifier.ABOVE)){
            pos.y = topRight.y;
        }
        if (position.Contains(CameraRelativityModifier.RIGHT)){
            pos.x = topRight.x;
        }
        if (position.Contains(CameraRelativityModifier.BELOW)){
            pos.y = bottomLeft.y;
        }
        if (position.Contains(CameraRelativityModifier.HORIZONTALMIDDLE)){
            pos.x = middle.x;
        }
        if (position.Contains(CameraRelativityModifier.VERTICALMIDDLE)){
            pos.y = middle.y;
        }

        pos.x += offset.x;
        pos.y += offset.y;
        return pos;
    }
    public override void Spawn(){
        Vector2 spawnPoint = GetSpawnPoint();

        Instantiate(prefab, spawnPoint, Quaternion.identity);
    }
}
