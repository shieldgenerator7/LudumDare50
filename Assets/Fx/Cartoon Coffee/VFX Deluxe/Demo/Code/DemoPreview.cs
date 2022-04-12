using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CartoonCoffee
{
    public class DemoPreview : MonoBehaviour
    {
        public static DemoPreview c;

        Dictionary<string, Button> buttons;
        Transform currentCategory;
        Transform categoryParent;

        Text currentParticle;
        Text currentIndex;
        int index;

        Transform particleParent;

        void Awake()
        {
            c = this;

            buttons = new Dictionary<string, Button>();
            currentParticle = transform.Find("Banner/CurrentText").GetComponent<Text>();
            currentIndex = transform.Find("Banner/Count").GetComponent<Text>();

            categoryParent = GameObject.Find("ParticleCategories").transform;

            particleParent = transform.parent.Find("Particles");

            GameObject catButton = transform.Find("Banner/CategoryButton").gameObject;

            for(int c = 0; c < categoryParent.childCount; c++)
            {
                Transform category = categoryParent.GetChild(c);

                GameObject newCatButton = Instantiate<GameObject>(catButton);
                newCatButton.GetComponent<Text>().text = category.name;
                newCatButton.transform.SetParent(catButton.transform.parent,false);
                newCatButton.GetComponent<RectTransform>().anchoredPosition = new Vector2(25, -80 - 27 * c);

                Button button = newCatButton.GetComponent<Button>();
                button.onClick.AddListener(delegate () { SelectCategory(category.name); });
                buttons.Add(category.name, button);

                newCatButton.SetActive(true);
                category.gameObject.SetActive(false);
            }

            //Select First Category:
            SelectCategory(categoryParent.GetChild(0).name);
        }

        private void Update()
        {
            if (currentCategory.name.StartsWith("Projectile")) return;

            if(Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.mouseScrollDelta.y < 0)
            {
                NextProjectile();
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.mouseScrollDelta.y > 0)
            {
                PreviousProjectile();
            }

            if ((Input.GetMouseButtonDown(0) && ((float)Input.mousePosition.x / (float)Screen.width) > (140f / 800f) || Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.KeypadEnter)) && currentCategory.name.StartsWith("Burst"))
            {
                GameObject newParticle = Instantiate<GameObject>(currentCategory.GetChild(index).GetChild(0).gameObject);
                newParticle.transform.SetParent(particleParent, false);
                Destroy(newParticle, 10);
            }
        }

        public void SelectCategory(string category)
        {
            if(currentCategory != null)
            {
                buttons[currentCategory.name].interactable = true;
                buttons[currentCategory.name].transform.Find("Selected").gameObject.SetActive(false);
                currentCategory.gameObject.SetActive(false);
            }

            buttons[category].interactable = false;
            buttons[category].transform.Find("Selected").gameObject.SetActive(true);

            currentCategory = categoryParent.Find(category);
            currentCategory.gameObject.SetActive(true);

            SelectIndex(0);
            RemoveButtonHighlight();
        }

        public void NextProjectile()
        {
            if (currentCategory.name.StartsWith("Projectile"))
            {
                DemoHandler.c.Next();
                return;
            }

            index++;
            if(index >= currentCategory.childCount)
            {
                index = 0;
            }

            SelectIndex(index);

            RemoveButtonHighlight();
        }
        public void PreviousProjectile()
        {
            if (currentCategory.name.StartsWith("Projectile"))
            {
                DemoHandler.c.Previous();
                return;
            }

            index--;
            if (index < 0)
            {
                index = currentCategory.childCount - 1;
            }

            SelectIndex(index);

            RemoveButtonHighlight();
        }

        void DisableOtherParticles()
        {
            for (int c = 0; c < currentCategory.childCount; c++)
            {
                currentCategory.GetChild(c).gameObject.SetActive(false);
            }

            for(int n = 0; n < particleParent.childCount; n++)
            {
                particleParent.GetChild(n).gameObject.SetActive(false);
            }
        }
        void SelectIndex(int newIndex)
        {
            DisableOtherParticles();
            index = newIndex;
            currentCategory.GetChild(index).gameObject.SetActive(true);

            UpdateText();
        }

        public void UpdateText()
        {
            if(currentCategory.name.StartsWith("Projectile"))
            {
                currentIndex.text = DemoHandler.c.GetIndexString();
                currentParticle.text = DemoHandler.c.GetProjectile();
            }
            else
            {
                currentIndex.text = (index + 1) + "/" + currentCategory.childCount;
                currentParticle.text = currentCategory.GetChild(index).name;
            }
        }

        void RemoveButtonHighlight()
        {
            if(EventSystem.current != null)
            {
                EventSystem.current.SetSelectedGameObject(null);
            }
        }
    }
}