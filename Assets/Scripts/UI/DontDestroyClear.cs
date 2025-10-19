using UnityEngine;
using UnityEngine.SceneManagement;

public static class DontDestroyCleaner
{
    public static void ClearAll()
    {
        // Cria uma cena temporária
        var tempScene = SceneManager.CreateScene("TempScene");

        // Cria um objeto temporário para conseguir mover os outros
        var tempObj = new GameObject("TempCleaner");

        // Move o objeto temporário para a cena DontDestroyOnLoad
        Scene dontDestroyScene = GetDontDestroyOnLoadScene();
        SceneManager.MoveGameObjectToScene(tempObj, dontDestroyScene);

        // Pega todos os root objects dessa cena
        GameObject[] rootObjects = dontDestroyScene.GetRootGameObjects();

        // Destroi todos, exceto o próprio tempObj (pra não causar erro)
        foreach (GameObject obj in rootObjects)
        {
            if (obj != tempObj)
                Object.Destroy(obj);
        }

        // Finalmente, destrói o temporário também
        Object.Destroy(tempObj);

        Debug.Log("🧹 Todos os objetos no DontDestroyOnLoad foram destruídos.");
    }

    // Função auxiliar para pegar a cena interna de DontDestroyOnLoad
    private static Scene GetDontDestroyOnLoadScene()
    {
        GameObject temp = new GameObject("SceneFinder");
        Object.DontDestroyOnLoad(temp);
        Scene scene = temp.scene;
        Object.DestroyImmediate(temp);
        return scene;
    }
}
