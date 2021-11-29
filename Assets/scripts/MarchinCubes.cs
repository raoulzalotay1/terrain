using System;
using System.Collections;
using System.Threading;
using Noise;
using UnityEngine;
using UnityEngine.Rendering;
using ThreadPriority = System.Threading.ThreadPriority;

namespace Assets
{
    public class MarchinCubes : MonoBehaviour
    {
        public FastNoiseSIMDUnity myNoise;

        private GameObject terrain;

        private readonly int[][] _table =
        {
            new int[] { },
            new[] {0, 8, 3},
            new[] {0, 1, 9},
            new[] {1, 8, 3, 9, 8, 1},
            new[] {1, 2, 10},
            new[] {0, 8, 3, 1, 2, 10},
            new[] {9, 2, 10, 0, 2, 9},
            new[] {2, 8, 3, 2, 10, 8, 10, 9, 8},
            new[] {3, 11, 2},
            new[] {0, 11, 2, 8, 11, 0},
            new[] {1, 9, 0, 2, 3, 11},
            new[] {1, 11, 2, 1, 9, 11, 9, 8, 11},
            new[] {3, 10, 1, 11, 10, 3},
            new[] {0, 10, 1, 0, 8, 10, 8, 11, 10},
            new[] {3, 9, 0, 3, 11, 9, 11, 10, 9},
            new[] {9, 8, 10, 10, 8, 11},
            new[] {4, 7, 8},
            new[] {4, 3, 0, 7, 3, 4},
            new[] {0, 1, 9, 8, 4, 7},
            new[] {4, 1, 9, 4, 7, 1, 7, 3, 1},
            new[] {1, 2, 10, 8, 4, 7},
            new[] {3, 4, 7, 3, 0, 4, 1, 2, 10},
            new[] {9, 2, 10, 9, 0, 2, 8, 4, 7},
            new[] {2, 10, 9, 2, 9, 7, 2, 7, 3, 7, 9, 4},
            new[] {8, 4, 7, 3, 11, 2},
            new[] {11, 4, 7, 11, 2, 4, 2, 0, 4},
            new[] {9, 0, 1, 8, 4, 7, 2, 3, 11},
            new[] {4, 7, 11, 9, 4, 11, 9, 11, 2, 9, 2, 1},
            new[] {3, 10, 1, 3, 11, 10, 7, 8, 4},
            new[] {1, 11, 10, 1, 4, 11, 1, 0, 4, 7, 11, 4},
            new[] {4, 7, 8, 9, 0, 11, 9, 11, 10, 11, 0, 3},
            new[] {4, 7, 11, 4, 11, 9, 9, 11, 10},
            new[] {9, 5, 4},
            new[] {9, 5, 4, 0, 8, 3},
            new[] {0, 5, 4, 1, 5, 0},
            new[] {8, 5, 4, 8, 3, 5, 3, 1, 5},
            new[] {1, 2, 10, 9, 5, 4},
            new[] {3, 0, 8, 1, 2, 10, 4, 9, 5},
            new[] {5, 2, 10, 5, 4, 2, 4, 0, 2},
            new[] {2, 10, 5, 3, 2, 5, 3, 5, 4, 3, 4, 8},
            new[] {9, 5, 4, 2, 3, 11},
            new[] {0, 11, 2, 0, 8, 11, 4, 9, 5},
            new[] {0, 5, 4, 0, 1, 5, 2, 3, 11},
            new[] {2, 1, 5, 2, 5, 8, 2, 8, 11, 4, 8, 5},
            new[] {10, 3, 11, 10, 1, 3, 9, 5, 4},
            new[] {4, 9, 5, 0, 8, 1, 8, 10, 1, 8, 11, 10},
            new[] {5, 4, 0, 5, 0, 11, 5, 11, 10, 11, 0, 3},
            new[] {5, 4, 8, 5, 8, 10, 10, 8, 11},
            new[] {9, 7, 8, 5, 7, 9},
            new[] {9, 3, 0, 9, 5, 3, 5, 7, 3},
            new[] {0, 7, 8, 0, 1, 7, 1, 5, 7},
            new[] {1, 5, 3, 3, 5, 7},
            new[] {9, 7, 8, 9, 5, 7, 10, 1, 2},
            new[] {10, 1, 2, 9, 5, 0, 5, 3, 0, 5, 7, 3},
            new[] {8, 0, 2, 8, 2, 5, 8, 5, 7, 10, 5, 2},
            new[] {2, 10, 5, 2, 5, 3, 3, 5, 7},
            new[] {7, 9, 5, 7, 8, 9, 3, 11, 2},
            new[] {9, 5, 7, 9, 7, 2, 9, 2, 0, 2, 7, 11},
            new[] {2, 3, 11, 0, 1, 8, 1, 7, 8, 1, 5, 7},
            new[] {11, 2, 1, 11, 1, 7, 7, 1, 5},
            new[] {9, 5, 8, 8, 5, 7, 10, 1, 3, 10, 3, 11},
            new[] {5, 7, 0, 5, 0, 9, 7, 11, 0, 1, 0, 10, 11, 10, 0},
            new[] {11, 10, 0, 11, 0, 3, 10, 5, 0, 8, 0, 7, 5, 7, 0},
            new[] {11, 10, 5, 7, 11, 5},
            new[] {10, 6, 5},
            new[] {0, 8, 3, 5, 10, 6},
            new[] {9, 0, 1, 5, 10, 6},
            new[] {1, 8, 3, 1, 9, 8, 5, 10, 6},
            new[] {1, 6, 5, 2, 6, 1},
            new[] {1, 6, 5, 1, 2, 6, 3, 0, 8},
            new[] {9, 6, 5, 9, 0, 6, 0, 2, 6},
            new[] {5, 9, 8, 5, 8, 2, 5, 2, 6, 3, 2, 8},
            new[] {2, 3, 11, 10, 6, 5},
            new[] {11, 0, 8, 11, 2, 0, 10, 6, 5},
            new[] {0, 1, 9, 2, 3, 11, 5, 10, 6},
            new[] {5, 10, 6, 1, 9, 2, 9, 11, 2, 9, 8, 11},
            new[] {6, 3, 11, 6, 5, 3, 5, 1, 3},
            new[] {0, 8, 11, 0, 11, 5, 0, 5, 1, 5, 11, 6},
            new[] {3, 11, 6, 0, 3, 6, 0, 6, 5, 0, 5, 9},
            new[] {6, 5, 9, 6, 9, 11, 11, 9, 8},
            new[] {5, 10, 6, 4, 7, 8},
            new[] {4, 3, 0, 4, 7, 3, 6, 5, 10},
            new[] {1, 9, 0, 5, 10, 6, 8, 4, 7},
            new[] {10, 6, 5, 1, 9, 7, 1, 7, 3, 7, 9, 4},
            new[] {6, 1, 2, 6, 5, 1, 4, 7, 8},
            new[] {1, 2, 5, 5, 2, 6, 3, 0, 4, 3, 4, 7},
            new[] {8, 4, 7, 9, 0, 5, 0, 6, 5, 0, 2, 6},
            new[] {7, 3, 9, 7, 9, 4, 3, 2, 9, 5, 9, 6, 2, 6, 9},
            new[] {3, 11, 2, 7, 8, 4, 10, 6, 5},
            new[] {5, 10, 6, 4, 7, 2, 4, 2, 0, 2, 7, 11},
            new[] {0, 1, 9, 4, 7, 8, 2, 3, 11, 5, 10, 6},
            new[] {9, 2, 1, 9, 11, 2, 9, 4, 11, 7, 11, 4, 5, 10, 6},
            new[] {8, 4, 7, 3, 11, 5, 3, 5, 1, 5, 11, 6},
            new[] {5, 1, 11, 5, 11, 6, 1, 0, 11, 7, 11, 4, 0, 4, 11},
            new[] {0, 5, 9, 0, 6, 5, 0, 3, 6, 11, 6, 3, 8, 4, 7},
            new[] {6, 5, 9, 6, 9, 11, 4, 7, 9, 7, 11, 9},
            new[] {10, 4, 9, 6, 4, 10},
            new[] {4, 10, 6, 4, 9, 10, 0, 8, 3},
            new[] {10, 0, 1, 10, 6, 0, 6, 4, 0},
            new[] {8, 3, 1, 8, 1, 6, 8, 6, 4, 6, 1, 10},
            new[] {1, 4, 9, 1, 2, 4, 2, 6, 4},
            new[] {3, 0, 8, 1, 2, 9, 2, 4, 9, 2, 6, 4},
            new[] {0, 2, 4, 4, 2, 6},
            new[] {8, 3, 2, 8, 2, 4, 4, 2, 6},
            new[] {10, 4, 9, 10, 6, 4, 11, 2, 3},
            new[] {0, 8, 2, 2, 8, 11, 4, 9, 10, 4, 10, 6},
            new[] {3, 11, 2, 0, 1, 6, 0, 6, 4, 6, 1, 10},
            new[] {6, 4, 1, 6, 1, 10, 4, 8, 1, 2, 1, 11, 8, 11, 1},
            new[] {9, 6, 4, 9, 3, 6, 9, 1, 3, 11, 6, 3},
            new[] {8, 11, 1, 8, 1, 0, 11, 6, 1, 9, 1, 4, 6, 4, 1},
            new[] {3, 11, 6, 3, 6, 0, 0, 6, 4},
            new[] {6, 4, 8, 11, 6, 8},
            new[] {7, 10, 6, 7, 8, 10, 8, 9, 10},
            new[] {0, 7, 3, 0, 10, 7, 0, 9, 10, 6, 7, 10},
            new[] {10, 6, 7, 1, 10, 7, 1, 7, 8, 1, 8, 0},
            new[] {10, 6, 7, 10, 7, 1, 1, 7, 3},
            new[] {1, 2, 6, 1, 6, 8, 1, 8, 9, 8, 6, 7},
            new[] {2, 6, 9, 2, 9, 1, 6, 7, 9, 0, 9, 3, 7, 3, 9},
            new[] {7, 8, 0, 7, 0, 6, 6, 0, 2},
            new[] {7, 3, 2, 6, 7, 2},
            new[] {2, 3, 11, 10, 6, 8, 10, 8, 9, 8, 6, 7},
            new[] {2, 0, 7, 2, 7, 11, 0, 9, 7, 6, 7, 10, 9, 10, 7},
            new[] {1, 8, 0, 1, 7, 8, 1, 10, 7, 6, 7, 10, 2, 3, 11},
            new[] {11, 2, 1, 11, 1, 7, 10, 6, 1, 6, 7, 1},
            new[] {8, 9, 6, 8, 6, 7, 9, 1, 6, 11, 6, 3, 1, 3, 6},
            new[] {0, 9, 1, 11, 6, 7},
            new[] {7, 8, 0, 7, 0, 6, 3, 11, 0, 11, 6, 0},
            new[] {7, 11, 6},
            new[] {7, 6, 11},
            new[] {3, 0, 8, 11, 7, 6},
            new[] {0, 1, 9, 11, 7, 6},
            new[] {8, 1, 9, 8, 3, 1, 11, 7, 6},
            new[] {10, 1, 2, 6, 11, 7},
            new[] {1, 2, 10, 3, 0, 8, 6, 11, 7},
            new[] {2, 9, 0, 2, 10, 9, 6, 11, 7},
            new[] {6, 11, 7, 2, 10, 3, 10, 8, 3, 10, 9, 8},
            new[] {7, 2, 3, 6, 2, 7},
            new[] {7, 0, 8, 7, 6, 0, 6, 2, 0},
            new[] {2, 7, 6, 2, 3, 7, 0, 1, 9},
            new[] {1, 6, 2, 1, 8, 6, 1, 9, 8, 8, 7, 6},
            new[] {10, 7, 6, 10, 1, 7, 1, 3, 7},
            new[] {10, 7, 6, 1, 7, 10, 1, 8, 7, 1, 0, 8},
            new[] {0, 3, 7, 0, 7, 10, 0, 10, 9, 6, 10, 7},
            new[] {7, 6, 10, 7, 10, 8, 8, 10, 9},
            new[] {6, 8, 4, 11, 8, 6},
            new[] {3, 6, 11, 3, 0, 6, 0, 4, 6},
            new[] {8, 6, 11, 8, 4, 6, 9, 0, 1},
            new[] {9, 4, 6, 9, 6, 3, 9, 3, 1, 11, 3, 6},
            new[] {6, 8, 4, 6, 11, 8, 2, 10, 1},
            new[] {1, 2, 10, 3, 0, 11, 0, 6, 11, 0, 4, 6},
            new[] {4, 11, 8, 4, 6, 11, 0, 2, 9, 2, 10, 9},
            new[] {10, 9, 3, 10, 3, 2, 9, 4, 3, 11, 3, 6, 4, 6, 3},
            new[] {8, 2, 3, 8, 4, 2, 4, 6, 2},
            new[] {0, 4, 2, 4, 6, 2},
            new[] {1, 9, 0, 2, 3, 4, 2, 4, 6, 4, 3, 8},
            new[] {1, 9, 4, 1, 4, 2, 2, 4, 6},
            new[] {8, 1, 3, 8, 6, 1, 8, 4, 6, 6, 10, 1},
            new[] {10, 1, 0, 10, 0, 6, 6, 0, 4},
            new[] {4, 6, 3, 4, 3, 8, 6, 10, 3, 0, 3, 9, 10, 9, 3},
            new[] {10, 9, 4, 6, 10, 4},
            new[] {4, 9, 5, 7, 6, 11},
            new[] {0, 8, 3, 4, 9, 5, 11, 7, 6},
            new[] {5, 0, 1, 5, 4, 0, 7, 6, 11},
            new[] {11, 7, 6, 8, 3, 4, 3, 5, 4, 3, 1, 5},
            new[] {9, 5, 4, 10, 1, 2, 7, 6, 11},
            new[] {6, 11, 7, 1, 2, 10, 0, 8, 3, 4, 9, 5},
            new[] {7, 6, 11, 5, 4, 10, 4, 2, 10, 4, 0, 2},
            new[] {3, 4, 8, 3, 5, 4, 3, 2, 5, 10, 5, 2, 11, 7, 6},
            new[] {7, 2, 3, 7, 6, 2, 5, 4, 9},
            new[] {9, 5, 4, 0, 8, 6, 0, 6, 2, 6, 8, 7},
            new[] {3, 6, 2, 3, 7, 6, 1, 5, 0, 5, 4, 0},
            new[] {6, 2, 8, 6, 8, 7, 2, 1, 8, 4, 8, 5, 1, 5, 8},
            new[] {9, 5, 4, 10, 1, 6, 1, 7, 6, 1, 3, 7},
            new[] {1, 6, 10, 1, 7, 6, 1, 0, 7, 8, 7, 0, 9, 5, 4},
            new[] {4, 0, 10, 4, 10, 5, 0, 3, 10, 6, 10, 7, 3, 7, 10},
            new[] {7, 6, 10, 7, 10, 8, 5, 4, 10, 4, 8, 10},
            new[] {6, 9, 5, 6, 11, 9, 11, 8, 9},
            new[] {3, 6, 11, 0, 6, 3, 0, 5, 6, 0, 9, 5},
            new[] {0, 11, 8, 0, 5, 11, 0, 1, 5, 5, 6, 11},
            new[] {6, 11, 3, 6, 3, 5, 5, 3, 1},
            new[] {1, 2, 10, 9, 5, 11, 9, 11, 8, 11, 5, 6},
            new[] {0, 11, 3, 0, 6, 11, 0, 9, 6, 5, 6, 9, 1, 2, 10},
            new[] {11, 8, 5, 11, 5, 6, 8, 0, 5, 10, 5, 2, 0, 2, 5},
            new[] {6, 11, 3, 6, 3, 5, 2, 10, 3, 10, 5, 3},
            new[] {5, 8, 9, 5, 2, 8, 5, 6, 2, 3, 8, 2},
            new[] {9, 5, 6, 9, 6, 0, 0, 6, 2},
            new[] {1, 5, 8, 1, 8, 0, 5, 6, 8, 3, 8, 2, 6, 2, 8},
            new[] {1, 5, 6, 2, 1, 6},
            new[] {1, 3, 6, 1, 6, 10, 3, 8, 6, 5, 6, 9, 8, 9, 6},
            new[] {10, 1, 0, 10, 0, 6, 9, 5, 0, 5, 6, 0},
            new[] {0, 3, 8, 5, 6, 10},
            new[] {10, 5, 6},
            new[] {11, 5, 10, 7, 5, 11},
            new[] {11, 5, 10, 11, 7, 5, 8, 3, 0},
            new[] {5, 11, 7, 5, 10, 11, 1, 9, 0},
            new[] {10, 7, 5, 10, 11, 7, 9, 8, 1, 8, 3, 1},
            new[] {11, 1, 2, 11, 7, 1, 7, 5, 1},
            new[] {0, 8, 3, 1, 2, 7, 1, 7, 5, 7, 2, 11},
            new[] {9, 7, 5, 9, 2, 7, 9, 0, 2, 2, 11, 7},
            new[] {7, 5, 2, 7, 2, 11, 5, 9, 2, 3, 2, 8, 9, 8, 2},
            new[] {2, 5, 10, 2, 3, 5, 3, 7, 5},
            new[] {8, 2, 0, 8, 5, 2, 8, 7, 5, 10, 2, 5},
            new[] {9, 0, 1, 5, 10, 3, 5, 3, 7, 3, 10, 2},
            new[] {9, 8, 2, 9, 2, 1, 8, 7, 2, 10, 2, 5, 7, 5, 2},
            new[] {1, 3, 5, 3, 7, 5},
            new[] {0, 8, 7, 0, 7, 1, 1, 7, 5},
            new[] {9, 0, 3, 9, 3, 5, 5, 3, 7},
            new[] {9, 8, 7, 5, 9, 7},
            new[] {5, 8, 4, 5, 10, 8, 10, 11, 8},
            new[] {5, 0, 4, 5, 11, 0, 5, 10, 11, 11, 3, 0},
            new[] {0, 1, 9, 8, 4, 10, 8, 10, 11, 10, 4, 5},
            new[] {10, 11, 4, 10, 4, 5, 11, 3, 4, 9, 4, 1, 3, 1, 4},
            new[] {2, 5, 1, 2, 8, 5, 2, 11, 8, 4, 5, 8},
            new[] {0, 4, 11, 0, 11, 3, 4, 5, 11, 2, 11, 1, 5, 1, 11},
            new[] {0, 2, 5, 0, 5, 9, 2, 11, 5, 4, 5, 8, 11, 8, 5},
            new[] {9, 4, 5, 2, 11, 3},
            new[] {2, 5, 10, 3, 5, 2, 3, 4, 5, 3, 8, 4},
            new[] {5, 10, 2, 5, 2, 4, 4, 2, 0},
            new[] {3, 10, 2, 3, 5, 10, 3, 8, 5, 4, 5, 8, 0, 1, 9},
            new[] {5, 10, 2, 5, 2, 4, 1, 9, 2, 9, 4, 2},
            new[] {8, 4, 5, 8, 5, 3, 3, 5, 1},
            new[] {0, 4, 5, 1, 0, 5},
            new[] {8, 4, 5, 8, 5, 3, 9, 0, 5, 0, 3, 5},
            new[] {9, 4, 5},
            new[] {4, 11, 7, 4, 9, 11, 9, 10, 11},
            new[] {0, 8, 3, 4, 9, 7, 9, 11, 7, 9, 10, 11},
            new[] {1, 10, 11, 1, 11, 4, 1, 4, 0, 7, 4, 11},
            new[] {3, 1, 4, 3, 4, 8, 1, 10, 4, 7, 4, 11, 10, 11, 4},
            new[] {4, 11, 7, 9, 11, 4, 9, 2, 11, 9, 1, 2},
            new[] {9, 7, 4, 9, 11, 7, 9, 1, 11, 2, 11, 1, 0, 8, 3},
            new[] {11, 7, 4, 11, 4, 2, 2, 4, 0},
            new[] {11, 7, 4, 11, 4, 2, 8, 3, 4, 3, 2, 4},
            new[] {2, 9, 10, 2, 7, 9, 2, 3, 7, 7, 4, 9},
            new[] {9, 10, 7, 9, 7, 4, 10, 2, 7, 8, 7, 0, 2, 0, 7},
            new[] {3, 7, 10, 3, 10, 2, 7, 4, 10, 1, 10, 0, 4, 0, 10},
            new[] {1, 10, 2, 8, 7, 4},
            new[] {4, 9, 1, 4, 1, 7, 7, 1, 3},
            new[] {4, 9, 1, 4, 1, 7, 0, 8, 1, 8, 7, 1},
            new[] {4, 0, 3, 7, 4, 3},
            new[] {4, 8, 7},
            new[] {9, 10, 8, 10, 11, 8},
            new[] {3, 0, 9, 3, 9, 11, 11, 9, 10},
            new[] {0, 1, 10, 0, 10, 8, 8, 10, 11},
            new[] {3, 1, 10, 11, 3, 10},
            new[] {1, 2, 11, 1, 11, 9, 9, 11, 8},
            new[] {3, 0, 9, 3, 9, 11, 1, 2, 9, 2, 11, 9},
            new[] {0, 2, 11, 8, 0, 11},
            new[] {3, 2, 11},
            new[] {2, 3, 8, 2, 8, 10, 10, 8, 9},
            new[] {9, 10, 2, 0, 9, 2},
            new[] {2, 3, 8, 2, 8, 10, 0, 1, 8, 1, 10, 8},
            new[] {1, 10, 2},
            new[] {1, 3, 8, 9, 1, 8},
            new[] {0, 9, 1},
            new[] {0, 3, 8},
            new int[] { }
        };

        private Vector3[] vertices = new Vector3[0];
        private int[] triangles = new int[0];

        [HideInInspector]
        public float[][][] _valuesGlobal;

        private bool AutoUpdate;
        private bool Interpolation;
        private long Seed = 1;
        private bool Shading;
        private int Size = 1;
        private float SurfaceValue = .5f;
        private float HeightMultiplier;

        public int XOffset;
        public int YOffset;
        public int ZOffset;

        public float Zoom = 1;

        private void Start()
        {
            StartCoroutine(StartThreaded());
        }

        private void LateUpdate()
        {
            SetMesh();
        }

        public IEnumerator StartThreaded()
        {
            AutoUpdate = terrainValues.AutoUpdate;
            Interpolation = terrainValues.Interpolation;
            Seed = terrainValues.Seed;
            Shading = terrainValues.Shading;
            Size = terrainValues.Size;
            SurfaceValue = terrainValues.SurfaceValue;
            Zoom = terrainValues.Zoom;
            HeightMultiplier = terrainValues.HeightMultiplier;
            myNoise = terrainValues.MyNoise;

            XOffset = (int)transform.position.x;
            YOffset = (int)transform.position.y;
            ZOffset = (int)transform.position.z;

            bool done = false;

            Thread thread = new Thread(() =>
            {
                Starter(out vertices, out triangles);
                done = true;
            })
            {
                Priority = ThreadPriority.BelowNormal
            };

            thread.Start();

            // Corountine waits for the thread to finish before continuing on the main thread
            while (!done)
                yield return null;

            CreateChunk.threadCount--;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.U) || AutoUpdate)
            {
                UpdateChunk();
            }
        }

        private void SetMesh()
        {
            Mesh mesh = new Mesh();

            mesh.SetVertices(vertices);
            mesh.SetTriangles(triangles, 0);

            mesh.indexFormat = IndexFormat.UInt32;

            mesh.Optimize();

            mesh.RecalculateNormals();
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        public void UpdateChunk()
        {
            AutoUpdate = terrainValues.AutoUpdate;
            Interpolation = terrainValues.Interpolation;
            Seed = terrainValues.Seed;
            Shading = terrainValues.Shading;
            Size = terrainValues.Size;
            SurfaceValue = terrainValues.SurfaceValue;
            Zoom = terrainValues.Zoom;

            XOffset = (int)transform.position.x;
            YOffset = (int)transform.position.y;
            ZOffset = (int)transform.position.z;

            UpdateMesh(_valuesGlobal, Size, new Mesh());

            var position = transform.position;

            var newPosition = new Vector3(Mathf.Floor(position.x), Mathf.Floor(position.y),
                Mathf.Floor(position.z));

            transform.position = newPosition;
        }

        private void Starter(out Vector3[] vertices, out int[] triangles)
        {
            var pointSize = Size + 1;
            _valuesGlobal = Perlin(pointSize);

            myNoise.seed = (int) Seed;

            CreateMesh(_valuesGlobal, Size, out vertices, out triangles);
        }

        private void UpdateMesh(float[][][] values, int size, Mesh emptyMesh)
        {
            var mesh = new Mesh();
            CreateMesh(values, size, out var vertices, out var triangles);

            mesh.vertices = vertices;
            mesh.triangles = triangles;

            mesh.Optimize();

            mesh.RecalculateNormals();
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        private float[][][] Perlin(int size)
        {
            var values = initializeFloatArray(size);
            float[] noiseSet = myNoise.fastNoiseSIMD.GetNoiseSet(XOffset, YOffset, ZOffset, size, size, size);

            int index = 0;
            for (int x = 0; x < size; x++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int z = 0; z < size; z++)
                    {
                        values[x][y][z] = (-y - YOffset) / HeightMultiplier + noiseSet[index++] + 1;
                    }
                }
            }
            return values;
        }

        float distance(Vector3 pos1, Vector3 pos2)
        {
            return (float)Math.Sqrt(Math.Pow(pos1.x - pos2.x, 2) + Math.Pow(pos1.y - pos2.y, 2) + Math.Pow(pos1.z - pos2.z, 2));
        }

        private float[][][] initializeFloatArray(int size)
        {
            var values = new float[size][][];

            for (var x = 0; x < size; x++)
            {
                values[x] = new float[size][];

                for (var i = 0; i < size; i++) values[x][i] = new float[size];
            }

            return values;
        }

        private void CreateMesh(float[][][] values, int size, out Vector3[] vertices, out int[] triangles)
        {
            vertices = initializeVector3Array(size * 2 + 1, size * 2 + 1, size * 2 + 1);
            triangles = new int[size * size * size * 3 * 2];

            var triangleCount = 0;

            for (var x = 0; x < size; x++)
                for (var y = 0; y < size; y++)
                    for (var z = 0; z < size; z++)
                    {
                        var triangleTable = _table[getTriangleIndex(
                            new[]
                            {
                        values[x][y][z],
                        values[x + 1][y][z],
                        values[x + 1][y][z + 1],
                        values[x][y][z + 1],
                        values[x][y + 1][z],
                        values[x + 1][y + 1][z],
                        values[x + 1][y + 1][z + 1],
                        values[x][y + 1][z + 1]
                            },
                            SurfaceValue)];

                        var trianglesToAdd = new int[triangleTable.Length];

                        for (var i = 0; i < triangleTable.Length; i += 3)
                        {
                            trianglesToAdd[i] = getVertexIndex(x, y, z, triangleTable[i], size * 2 + 2);
                            trianglesToAdd[i + 1] = getVertexIndex(x, y, z, triangleTable[i + 1], size * 2 + 2);
                            trianglesToAdd[i + 2] = getVertexIndex(x, y, z, triangleTable[i + 2], size * 2 + 2);
                        }

                        for (var i = 0; i < trianglesToAdd.Length; i++)
                        {
                            triangles[triangleCount] = trianglesToAdd[i];
                            triangleCount++;
                        }
                    }

            if (Interpolation) vertices = addIndterpolation(vertices, size * 2 + 2, values, SurfaceValue);

            //vertices = ScaleVertices(vertices);

            if (Shading == false) RemakeMeshToDiscrete(vertices, triangles, out vertices, out triangles);

            this.vertices = vertices;
            this.triangles = triangles;
        }

        private Vector3[] ScaleVertices(Vector3[] vertices)
        {
            for (var i = 0; i < vertices.Length; i++)
            {
                vertices[i].x = vertices[i].x / transform.localScale.x;
                vertices[i].y = vertices[i].y / transform.localScale.y;
                vertices[i].z = vertices[i].z / transform.localScale.z;
            }

            return vertices;
        }

        private void RemakeMeshToDiscrete(Vector3[] vert, int[] trig, out Vector3[] outVertices, out int[] outTriangles)
        {
            var vertDiscrete = new Vector3[trig.Length];
            var trigDiscrete = new int[trig.Length];
            for (var i = 0; i < trig.Length; i++)
            {
                vertDiscrete[i] = vert[trig[i]];
                trigDiscrete[i] = i;
            }

            outVertices = vertDiscrete;
            outTriangles = trigDiscrete;
        }

        private Vector3[] addIndterpolation(Vector3[] array, int sizeVertices, float[][][] values, float surfaceValue)
        {
            var sizeValues = values.Length;
            for (var x = 0; x < sizeValues; x++)
                for (var y = 0; y < sizeValues; y++)
                    for (var z = 0; z < sizeValues; z++)
                    {
                        var arrayCords = z * 2 + y * sizeVertices * 2 + x * sizeVertices * sizeVertices * 2;

                        var value = values[x][y][z];

                        if (z < sizeValues - 1)
                        {
                            var valueX = values[x][y][z + 1];
                            var interpolationValueX = Mathf.Abs(surfaceValue - value) /
                                                      (Mathf.Abs(surfaceValue - value) + Mathf.Abs(surfaceValue - valueX));
                            array[arrayCords + 1] += new Vector3(0, 0, interpolationValueX - .5f);
                        }

                        if (y < sizeValues - 1)
                        {
                            var valueY = values[x][y + 1][z];
                            var interpolationValueY = Mathf.Abs(surfaceValue - value) /
                                                      (Mathf.Abs(surfaceValue - value) + Mathf.Abs(surfaceValue - valueY));
                            array[arrayCords + sizeVertices] += new Vector3(0, interpolationValueY - .5f, 0);
                        }

                        if (x < sizeValues - 1)
                        {
                            var valueZ = values[x + 1][y][z];
                            var interpolationValueZ = Mathf.Abs(surfaceValue - value) /
                                                      (Mathf.Abs(surfaceValue - value) + Mathf.Abs(surfaceValue - valueZ));
                            array[arrayCords + sizeVertices * sizeVertices] += new Vector3(interpolationValueZ - .5f, 0, 0);
                        }
                    }

            return array;
        }

        private int getVertexIndex(int z, int y, int x, int cubeValue, int size)
        {
            int[][] verticeValues =
            {
                new[] {1, 0, 0},
                new[] {2, 0, 1},
                new[] {1, 0, 2},
                new[] {0, 0, 1},
                new[] {1, 2, 0},
                new[] {2, 2, 1},
                new[] {1, 2, 2},
                new[] {0, 2, 1},
                new[] {0, 1, 0},
                new[] {2, 1, 0},
                new[] {2, 1, 2},
                new[] {0, 1, 2}
            };

            var xReturn = z * 2 * size * size;
            var yReturn = y * 2 * size;
            var zReturn = x * 2;
            return xReturn + verticeValues[cubeValue][0] * size * size + yReturn + verticeValues[cubeValue][1] * size +
                   zReturn + verticeValues[cubeValue][2];
        }

        private int getTriangleIndex(float[] values, float surfaceValue)
        {
            var index = 0;

            for (var i = 0; i < 8; i++)
                if (values[i] > surfaceValue)
                    index |= 1 << i;
            return index;
        }

        private Vector3[] initializeVector3Array(int xsize, int ysize, int zsize)
        {
            var array = new Vector3[(xsize + 1) * (ysize + 1) * (zsize + 1)];
            var count = 0;
            for (var x = 0; x < xsize + 1; x++)
                for (var y = 0; y < ysize + 1; y++)
                    for (var z = 0; z < zsize + 1; z++)
                    {
                        array[count] = new Vector3(x / 2f, y / 2f, z / 2f);
                        count++;
                    }

            return array;
        }
    }
}