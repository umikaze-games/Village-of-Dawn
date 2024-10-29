using UnityEditor;
using UnityEngine;

// CLass to change the sprite  
[ExecuteInEditMode] // This allows the script to run in Edit mode
public class SpriteSelectFrame : MonoBehaviour
{
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component
    private Sprite[] sprites; // Array to hold the sprites automatically from the sprite sheet

    // Public variable to set the sprite index from Inspector
    public int spriteIndex = 0; // sprite index from the spritesheet
    public string spritesheetLocation = "Prefabs/Crops/Sprites"; // folder location, from Resources/

    // ===================================================
    // update the sprite in the Scene in Edit Mode
    void OnEnable()
    {
        loadVariables();
        ChangeSprite(spriteIndex);
    }

    void OnValidate()
    {
        // Perform basic validation or assignments

        #if UNITY_EDITOR
        // Delay the call until it's safe
        EditorApplication.delayCall += () =>
        {
            if (this != null) // Ensure object still exists
            {
                loadVariables();
                if (sprites != null && sprites.Length > spriteIndex)
                {
                    spriteRenderer.sprite = sprites[spriteIndex];
                }
            }
        };
        #endif
    }

    // ===================================================
    // update the sprite in Play Mode
    void Start()
    {
        // load variables and change the sprite accordingly
        loadVariables();
        ChangeSprite(spriteIndex);
    }
    void Update()
    {
        // change the sprite
        ChangeSprite(spriteIndex);
    }

    // ===================================================
    // helpers
    
    // load sprites from a spritesheet
    void LoadSpritesFromSpriteSheet(Sprite spriteSheet)
    {
        // locate the spritesheet
        string gameobjectBaseName = spriteRenderer.gameObject.name; // base name is ~ "crop_01"
        if (gameobjectBaseName.IndexOf(" (") > 0) // check is base name is ~ "crop_01 (1)", then clean it
            gameobjectBaseName = gameobjectBaseName.Substring(0, gameobjectBaseName.IndexOf(" ("));
        string spriteSheetName = gameobjectBaseName; // fallback 
        if (spriteSheet != null)
            spriteSheetName = spriteSheet.texture.name;
        string spriteSheetPath = spritesheetLocation + "/" + spriteSheetName;
        sprites = Resources.LoadAll<Sprite>(spriteSheetPath);
    }

    // load the spritesheet
    void loadVariables()
    {
        Debug.Log("hello");
        spriteRenderer = GetComponent<SpriteRenderer>();
        LoadSpritesFromSpriteSheet(spriteRenderer.sprite);
    }
    // update the sprite
    public void ChangeSprite(int index)
    {
        if (index >= 0 && index < sprites.Length)
            spriteRenderer.sprite = sprites[index];
    }
}
