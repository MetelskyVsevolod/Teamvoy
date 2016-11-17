using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEditor;
using System.Collections;

public class ListManager : MonoBehaviour
{
    public GameObject[] animatedObjects;
    public Transform objectsListParent, animatedObjectParent;
    public GameObject listItemPrefab;
    public Text sliderValue;
    public Slider slider;
    public Text playAnimationText, ObjectText;
    public Camera reviewCamera;

    int indexOfSelectedObject = 0;

    bool isAnimationPlaying = false;

    public bool IsAnimationPlaying
    {
        get { return isAnimationPlaying; }
        set
        {
            isAnimationPlaying = value;
            playAnimationText.text = (isAnimationPlaying) ? "Stop animation" : "Play animation";
        }
    }

    public int IndexOfSelectedObject
    {
        get { return indexOfSelectedObject; }
        set
        {
            if (indexOfSelectedObject > -1)
            { 
                animatedObjects[indexOfSelectedObject].SetActive(false);
                objectsListParent.GetChild(indexOfSelectedObject).GetComponent<Image>().color = Color.white;
            }

            indexOfSelectedObject = value;
            animatedObjects[indexOfSelectedObject].SetActive(true);

            IsAnimationPlaying = false;

            ObjectText.text = animatedObjects[indexOfSelectedObject].name;

            objectsListParent.GetChild(indexOfSelectedObject).GetComponent<Image>().color = Color.cyan;
        }
    }

    float animationSpeed = 1f;

    public float AnimationSpeed
    {
        get { return animationSpeed; }
        set
        {
            animationSpeed = value;

            if (indexOfSelectedObject > -1)
                animatedObjects[indexOfSelectedObject].GetComponent<Animator>().speed = value;
        }
    }

    void hideListOfAnimatedObjects()
    {
        for (int i = 0; i < objectsListParent.childCount; i++)
        {
            objectsListParent.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void spawnAnimatedObjects()
    {
        AssetPreview.SetPreviewTextureCacheSize(1024);

        for (int i = 0; i < animatedObjects.Length; i++)
        {
            GameObject newAnimatedObject = Instantiate(animatedObjects[i]) as GameObject;
            newAnimatedObject.transform.SetParent(animatedObjectParent);
            newAnimatedObject.transform.localScale = Vector3.one;
            newAnimatedObject.transform.localPosition = Vector3.zero;
            newAnimatedObject.gameObject.name = newAnimatedObject.gameObject.name.Replace("(Clone)", "");
            animatedObjects[i] = newAnimatedObject;
            newAnimatedObject.layer = 8;
            animatedObjects[i].SetActive(false);
        }
        AssetPreview.GetAssetPreview(animatedObjects[0]);
    }

    public void showListOfAnimatedObjects()
    {
        hideListOfAnimatedObjects();

        for (int i = 0; i < animatedObjects.Length; i++)
        {
            if (i >= objectsListParent.childCount)
            {
                GameObject newListItem = Instantiate(listItemPrefab) as GameObject;
                newListItem.gameObject.name = newListItem.gameObject.name.Replace("(Clone)", "_" + i);
                newListItem.transform.SetParent(objectsListParent);
                newListItem.transform.localScale = Vector3.one;
                newListItem.transform.localPosition = Vector3.zero;
            }

            Transform animatedObjectslot = objectsListParent.GetChild(i);

            animatedObjectslot.gameObject.SetActive(true);

            Text listItemName = animatedObjectslot.FindChild("listItemText").GetComponent<Text>();

            listItemName.text = animatedObjects[i].name;

            RawImage listItemPreview = animatedObjectslot.FindChild("listItemPreview").GetComponent<RawImage>();

            animatedObjects[i].SetActive(true);

            listItemPreview.texture = PreviewMaker.takeScreenshot(reviewCamera);

            animatedObjects[i].SetActive(false);

            Button button = animatedObjectslot.GetComponent<Button>();

            int tempIndexContainter = i;

            button.onClick.AddListener(delegate { IndexOfSelectedObject = tempIndexContainter; });
        }

        if(animatedObjects.Length > 0)
            IndexOfSelectedObject = 0;
    }

    public void playStopAnimation()
    {
        animatedObjects[indexOfSelectedObject].GetComponent<Animator>().speed = AnimationSpeed;
        animatedObjects[indexOfSelectedObject].GetComponent<Animator>().SetTrigger("play");
        IsAnimationPlaying = !IsAnimationPlaying;
    }

    public void onSliderValueChanged()
    {
        sliderValue.text = slider.value.ToString();
        AnimationSpeed = slider.value;
    }
}
