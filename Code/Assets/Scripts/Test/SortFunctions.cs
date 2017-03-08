using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SortFunctions : MonoBehaviour
{
    private bool active = false;

    int quickSortFuncCall = 0;

    int mergeSortFuncCall = 0;

	void Start () {
        int[] arr = CreateArr();

        ShellSort_spec_gap(arr, arr.Length);

        arr = CreateArr();

        float t = Time.realtimeSinceStartup;

        QuickSort(arr, 0, arr.Length - 1);

        Debug.Log("QuickSort : " + (Time.realtimeSinceStartup - t));
        Debug.Log("quickSortFuncCall = " + quickSortFuncCall);

        arr = CreateArr();

        ShellSort_half_gap(arr, arr.Length);

        arr = CreateArr();

        //DebugArray("Before MergeSort : ", arr);

        t = Time.realtimeSinceStartup;

        int[] temp = new int[arr.Length];

        MergeSort(arr, 0, arr.Length - 1,temp);

        Debug.Log("MergeSort : " + (Time.realtimeSinceStartup - t));
        Debug.Log("mergeSortFuncCall = " + mergeSortFuncCall);

        //DebugArray("After MergeSort : ", arr);
	}

    int[] CreateArr()
    {
        //int[] arr = new int[] { 99, 10, 39, 50, 79, 20, 13, 17, 5, 9, 30, 11, 16 };
        //return arr;

        int[] arr = new int[100000];

        int len = arr.Length;
        int counter = 0;

        for (int i = len - 1 ; i >= 0 ; i--)
        {
            arr[i] = counter++;
        }
        return arr;
    }

    void MyDebug(string msg) {
        Debug.Log(gameObject.name + " | " + msg);
    }

    void Check(int[] source)
    {
        bool isUpperSeq = true;
        for (int i = 0 ; i < source.Length - 1 ; i++)
        {
            if (source[i] > source[i + 1])
                isUpperSeq = false;
        }
        Debug.Log("IsUppgerSeq = " + isUpperSeq);
    }

    void ShellSort_spec_gap(int[] array, int length)
    {
        int d = 1;   //设置希尔排序的增量

        while (d < length / 3)
        {
            d = d * 3 + 1; //增量间隔  
        }

        float begin = Time.realtimeSinceStartup;

        while(d>=1)    
        {
            InserSort(array, d);

            //DebugArray("d = " + d + "array = ", array);

            d = (d-1) / 3;
        }
        Debug.Log("ShellSort_spec_gap = " + (Time.realtimeSinceStartup - begin));
        //Check(array);
    }

    void InserSort(int[] source, int d)
    {
        int nElem = source.Length;
        for (int i = d ; i < nElem ; i++)
        {
            int temp = source[i];
            int j = i - d;
            while (j >= 0 && source[j] > temp)
            {
                source[j + d] = source[j];
                j = j - d;
            }
            source[j + d] = temp;
        }
    }

    void ShellSort_half_gap(int[] array, int length)
    {
        int d = length / 2;   //设置希尔排序的增量

        float begin = Time.realtimeSinceStartup;

        while (d >= 1)
        {
            InserSort(array, d);
            d= d/2;    //缩小增量    
        }
        Debug.Log("ShellSort_half_gap = " + (Time.realtimeSinceStartup - begin));
        //Check(array);
    }

    void DebugArray(string msg , int[] source)
    {
        string str = "";
        foreach (int e in source)
        {
            str += e + ", ";
        }
        Debug.Log(msg + " : " + str);
    }

    void QuickSort(int[] src, int l, int r)
    {
        quickSortFuncCall++;
        if (l < r)
        {
            int i = l;
            int j = r;

            int temp = src[i];
            src[i] = src[(i + j) / 2];
            src[(i + j) / 2] = temp;

            int x = src[i];

            while (i < j)
            {
                while (i < j && src[j] >= x)
                    j--;
                if (i < j)
                    src[i++] = src[j];

                while (i < j && src[i] <= x)
                    i++;
                if (i < j)
                    src[j--] = src[i];
            }

            src[i] = x;

            QuickSort(src, l, i - 1);
            QuickSort(src, i + 1, r);
        }

    }

    int AdjustArray(int[] src, int l, int r)
    {
        int i = l;
        int j = r;
        int x = src[i];

        while (i < j)
        {
            while (i < j && src[j] >= x)
                j--;
            if (i < j)
                src[i++] = src[j];

            while (i < j && src[i] <= x)
                i++;
            if (i < j)
                src[j--] = src[i];
        }

        src[i] = x;

        return i;
    }

    void MergeSort(int[] src, int first, int last, int[] temp)
    {
        mergeSortFuncCall++;
        if (first < last)
        {
            int mid = (first + last) / 2;
            MergeSort(src, first, mid, temp);
            MergeSort(src, mid + 1, last, temp);
            MergeArray(src, first, mid, last, temp);
        }
    }

    void MergeArray(int[] src, int first, int mid, int last, int[] temp)
    {
        int i = first;
        int j = mid + 1;
        int m = mid;
        int n = last;
        int k = 0;

        while (i <= m && j <= n)
        {
            if (src[i] < src[j])
                temp[k++] = src[i++];
            else
                temp[k++] = src[j++];
        }

        while (i <= m)
            temp[k++] = src[i++];

        while (j <= n)
            temp[k++] = src[j++];

        for (i = 0 ; i < k ; i++)
        {
            src[first + i] = temp[i];
        }

        //DebugArray("MergeArray", src);

    }

	void Update () {
	}
}
