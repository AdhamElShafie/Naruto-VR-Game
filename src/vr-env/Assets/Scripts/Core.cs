using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Security.Permissions;
using UnityEngine;

public class Core : MonoBehaviour
{
    public string fileToWatch = "/moves.txt";
    public string path = "../app";
    public string jitsu = "";
    public AudioSource jitsuAudio;
    public AudioSource windAudio;

    private bool _inStatusChange = false;
    private bool _isComboFound = false;

    public GameObject ExplosionPrefab;
    public GameObject RockPrefab;
    public GameObject SandPrefab;
    public GameObject WaterfallPrefab;
    public WindZone WindPrefab;

    private FileSystemWatcher watcher;
    [PermissionSet(SecurityAction.Demand, Name="FullTrust")]

    // Start is called before the first frame update
    // Monitored file located at: ../src/app/moves.txt
    void Start()
    {
        watcher = new FileSystemWatcher();
        watcher.Path = "../app";
        watcher.NotifyFilter = NotifyFilters.LastWrite;
        watcher.Filter = "moves.txt";
        watcher.Changed += new FileSystemEventHandler(OnChanged);
        watcher.EnableRaisingEvents = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isComboFound) {
            _isComboFound = false;
            switch (jitsu)
            {
                case "1":
                    SpawnRock();
                    break;
                case "2":
                    WindEffect();
                    break;
                case "3":
                    SpawnWaterfall();
                    break;
                case "4":
                    SpawnExplosion();
                    break;
                case "5":
                    SandSwirlEffect();
                    break;
                default:
                    break;
            }
        }
    }

    public void OnChanged(object source, FileSystemEventArgs e)
    {
        if (_inStatusChange)
        {
            return;
        }
        else
        {
            _inStatusChange = true;

            print("File has changed: " +  e.Name + " " + e.FullPath);

            using (StreamReader reader = new StreamReader(e.FullPath)){
                jitsu = reader.ReadLine();
            }

            _isComboFound = true;

            _inStatusChange = false;
        }
    }

    public void SandSwirlEffect()
    {
        jitsuAudio.Play(0);
        SandPrefab.transform.localScale += new Vector3(20, 20, 20);
        Vector3 playerPos = transform.position;
        Vector3 playerDirection = transform.forward;
        float spawnDistance = 20;

        Vector3 spawnPos = playerPos + playerDirection*spawnDistance;
        Instantiate(SandPrefab, spawnPos, Quaternion.identity);
    }

    public void SpawnExplosion()
    {
        jitsuAudio.Play(0);
        Vector3 playerPos = transform.position;
        Vector3 playerDirection = transform.forward;
        float spawnDistance = 20;

        Vector3 spawnPos = playerPos + playerDirection*spawnDistance;
        Instantiate(ExplosionPrefab, spawnPos, Quaternion.identity);
    }

    public void SpawnRock()
    {
        jitsuAudio.Play(0);
        Vector3 playerPos = transform.position;
        Vector3 playerDirection = transform.forward;
        float spawnDistance = 20;

        Vector3 spawnPos = playerPos + playerDirection*spawnDistance;
        Instantiate(RockPrefab, spawnPos, Quaternion.identity);
    }

    public void SpawnWaterfall()
    {
        jitsuAudio.Play(0);
        Vector3 playerPos = transform.position;
        Vector3 playerDirection = transform.forward;
        float spawnDistance = 20;

        Vector3 spawnPos = playerPos + playerDirection*spawnDistance;
        spawnPos.y = 0.0f;
        Instantiate(WaterfallPrefab, spawnPos, Quaternion.identity);
    }

    public void WindEffect()
    {
        jitsuAudio.Play(0);
        if (WindPrefab.windMain == 3) {
            windAudio.Pause();
            WindPrefab.windMain = 0.0f;
            WindPrefab.windTurbulence = 0.0f;
        } else {
            windAudio.Play(0);
            WindPrefab.windMain += 1.0f;
            WindPrefab.windTurbulence += 1.0f;
        }
    }
}
