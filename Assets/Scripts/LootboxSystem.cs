using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class LootboxSystem : MonoBehaviour
{
    [SerializeField] private GameObject _spriteArrow;
    [SerializeField] private ItemView _itemViewPrefab;
    [SerializeField] private BoxScriptable _boxScriptable;

    [Header("View Settings")] [SerializeField]
    private int _xOffset;

    [SerializeField] private int _viewsAmount;
    [SerializeField] private int _viewSize;

    [SerializeField] private float _animationSpeed;
    [SerializeField] private int _animationDuration;

    [SerializeField] private SpriteRenderer _prizePanel;
    [SerializeField] private SpriteRenderer _prizeSprite;

    private List<BoxItem> _tempInternalBoxItemsList;

    public void StartRoll()
    {
        _tempInternalBoxItemsList = new List<BoxItem>(_boxScriptable.ItemsToDrop);
        _tempInternalBoxItemsList = _tempInternalBoxItemsList.OrderBy(x => x.DropChance).ToList();
        InstantiateViews();
    }

    private void InstantiateViews()
    {
        var tempOffset = _xOffset;
        var maxXOffset = _xOffset + _viewSize;
        var respawnPosition = _xOffset - (_viewsAmount - 1) * _viewSize;
        var currentViewsList = new List<ItemView>();
        
        for (int i = 0; i < _viewsAmount; i++)
        {
            var view = Instantiate(_itemViewPrefab, new Vector3(tempOffset, 0, 0), Quaternion.identity, transform);
            var boxItem = GetRandomItem();
            view.SetData(maxXOffset, respawnPosition, _animationSpeed, boxItem.ItemSO.Name, boxItem.ItemSO.Color);
            currentViewsList.Add(view);
            tempOffset -= _viewSize;
        }

        foreach (var view in currentViewsList)
        {
            view.SetMove(true);
        }

        _spriteArrow.SetActive(true);
        StartCoroutine(MovementFade(currentViewsList));
    }

    private IEnumerator MovementFade(List<ItemView> itemViewsList)
    {
        var tempSpeed = _animationSpeed;
        yield return new WaitForSeconds(_animationDuration);

        while (tempSpeed >= 0)
        {
            tempSpeed -= 0.1f;
            foreach (var view in itemViewsList)
            {
                view.SetSpeed(tempSpeed);
            }

            yield return new WaitForSeconds(0.1f);
        }

        foreach (var view in itemViewsList)
        {
            view.SetSpeed(0);
            view.SetMove(false);
        }

        FindWinner(itemViewsList);
    }

    private void FindWinner(List<ItemView> itemViewsLis)
    {
        float closestView = 999f;
        ItemView winnerView = null;

        foreach (var view in itemViewsLis)
        {
            var distance = Vector3.Distance(_spriteArrow.transform.position, view.transform.position);
            if (distance < closestView)
            {
                closestView = distance;
                winnerView = view;
            }
        }

        ShowPrize(winnerView);
    }

    private void ShowPrize(ItemView view)
    {
        StartCoroutine(ShowPrizeAnimation(view));
    }

    private IEnumerator ShowPrizeAnimation(ItemView view)
    {
        float alpha = 0;

        var prizePanelColor = _prizePanel.color;
        var prizeViewColor = view.GetComponent<SpriteRenderer>().color;
        while (alpha <= 1)
        {
            alpha += 0.01f;

            prizePanelColor = new Color(prizePanelColor.r, prizePanelColor.g, prizePanelColor.b, alpha);
            prizeViewColor = new Color(prizeViewColor.r, prizeViewColor.g, prizeViewColor.b, alpha);
            _prizePanel.color = prizePanelColor;
            _prizeSprite.color = prizeViewColor;
            yield return null;
        }
    }

    private BoxItem GetRandomItem()
    {
        var maxChance = _boxScriptable.TotalWeight + 1;
        var randomValue = Random.Range(0, maxChance);
        var currentChance = 0;

        for (int i = 0; i < _tempInternalBoxItemsList.Count; i++)
        {
            currentChance += _tempInternalBoxItemsList[i].DropChance;

            if (randomValue <= currentChance)
            {
                return _tempInternalBoxItemsList[i];
            }
        }

        Debug.LogError("Error while geting item");
        return null;
    }
}