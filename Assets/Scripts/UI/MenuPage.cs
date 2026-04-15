using UnityEngine;

public class MenuPage : MonoBehaviour {
    public bool deactivateOnStart;
    private MenuPage previousPage;

    private void Start() {
        if (deactivateOnStart) gameObject.SetActive(false);
        transform.localPosition = Vector3.zero;
    }

    //Botão da Página anterior pega referência da próxima página pelo page
    public void Open(MenuPage page) {
        page.gameObject.SetActive(true);
        page.previousPage = this;
    }

    public void Close() {
        gameObject.SetActive(false);
        if(previousPage) previousPage.gameObject.SetActive(true);
    }
}
