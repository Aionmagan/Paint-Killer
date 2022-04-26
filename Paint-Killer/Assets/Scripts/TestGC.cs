/// Test program for GC.GetTotalMemory.
/// Yossarian King / Blackbird Interactive Inc. / December 2016
///
/// No form of copyright asserted, use as you see fit.

using System;
using System.Collections.Generic;

using UnityEngine;

public class TestGC : MonoBehaviour
{
    // (4096 - 40) causes 4096 byte allocation per block; +1 will allocate 8192 bytes per block
    // (1024 - 40) causes 4096 byte allocation per every 4  blocks; +1 will allocate 4096 bytes every 3 blocks
    // Smaller blocks (e.g. 512 and below) still show the 4K deltas, but the overhead seems to be higher, not sure why?
    // Results are consistent between 64-bit Unity Editor (edit time or  play mode) and x64 Windows standalone build.
    // Tested with Unity 5.4.3f1.
    [SerializeField]int BlockSize = 4056;
    [SerializeField]int BlockCount = 100;

    // GUI method to do the test while running.
    private void OnGUI()
    {
        if (GUILayout.Button("TestTotalMemory"))
        {
            TestTotalMemory();
        }
        GUILayout.Label("Block Size:");
        BlockSize = Int32.Parse(GUILayout.TextField(BlockSize.ToString()));
        GUILayout.Label("Block Count:");
        BlockCount = Int32.Parse(GUILayout.TextField(BlockCount.ToString()));
    }

    // Context menu to do the test from editor (via Inspector context menu).
    [ContextMenu("TestTotalMemory")]
    private void TestTotalMemory()
    {
        byte[][] blocks = new byte[BlockCount][];
        long[] blockMemoryDiff = new long[BlockCount];

        GC.Collect();
        long previousTotalMemory = System.GC.GetTotalMemory(true);
        for (int i = 0; i < BlockCount; ++i)
        {
            blocks[i] = new byte[BlockSize];
            GC.Collect();
            long newTotalMemory = GC.GetTotalMemory(true);
            blockMemoryDiff[i] = newTotalMemory - previousTotalMemory;
            previousTotalMemory = newTotalMemory;
        }

        Debug.LogFormat("=== BLOCK SIZE {0}; BLOCK COUNT {1} ===", BlockSize, BlockCount);
        long sumOfBlocks = 0;
        for (int i = 0; i < BlockCount; ++i)
        {
            sumOfBlocks += BlockSize;
            if (blockMemoryDiff[i] != 0)
            {
                Debug.LogFormat("{0}: diff={1} (expected={2})", i, blockMemoryDiff[i], sumOfBlocks);
                sumOfBlocks = blocks[i][0];     // HACK: reference an element in a block to make sure GC can't reclaim it before we gather allocation data.
            }
        }
    }
}
