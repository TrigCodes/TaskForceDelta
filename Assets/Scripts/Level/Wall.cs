/***************************************************************
*file: Wall.cs
*author: Samin Hossain, An Le, Otto Del Cid, Luis Navarrete, Luis Salazar, Sebastian Cursaro
*class: CS 4700 - Game Development
*assignment: Final Program
*date last modified: 5/6/2024
*
*purpose: This class provide general behavior for wall
*
****************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    [Header("Wall Attributes")]
    [SerializeField] public float wallRegenTimer = 60f; // Timer for wall regeneration

}
