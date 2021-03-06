﻿using UnityEngine;
using System.Linq;
using System;

public class CameraFollow : MonoBehaviour
{
    private GameManager m_GameManager;
    private Camera m_Camera;

    private Vector3 m_DesiredPos;
    private float m_DesiredSize;

    [SerializeField]
    private float LerpSpeed = 0.3f;

    void Start()
    {
        m_GameManager = GameObject.FindGameObjectWithTag("GameController").GetComponent<GameManager>();
        m_Camera = GetComponent<Camera>();

        if (!m_GameManager)
            Debug.LogError("No GameManager found, game manager should have 'GameController' tag. Camera follow will be ignored.");
    }

    void Update()
    {
        if (!m_GameManager)
            return;
        
        Vector3[] players;
        try
        {
            players = m_GameManager.players.Where(p => p == true).Select(p => p.transform.position).ToArray();
        }
        catch(Exception ex)
        {
            return;
        }


        if (players == null || players.Length <= 0)
            return;

        var minX = players.Select(p => p.x).Min();
        var maxX = players.Select(p => p.x).Max();
        var xDiff = maxX - minX;

        var minY = players.Select(p => p.y).Min();
        var maxY = players.Select(p => p.y).Max();
        var yDiff = maxY - minY;

        m_DesiredPos = new Vector3((maxX + minX) / 2, (maxY + minY) / 2, -10);

        m_DesiredSize = xDiff > yDiff ? xDiff / 2  : yDiff / 2;
        m_DesiredSize *= xDiff > yDiff ? 9/16f : 1;
        m_DesiredSize += (m_DesiredSize < 10) ? (10 - m_DesiredSize) / 2 : 0;
        m_DesiredSize += 2;
        m_DesiredSize = m_DesiredSize > 5 ? m_DesiredSize : 5;
        m_DesiredSize = m_DesiredSize > 5 ? m_DesiredSize : 5;

        PerformLerp();
    }

    private void PerformLerp()
    {
        m_Camera.orthographicSize -= (m_Camera.orthographicSize - m_DesiredSize) * LerpSpeed;
        m_Camera.transform.position = Vector3.Lerp(m_Camera.transform.position, m_DesiredPos, LerpSpeed);
    }
}
