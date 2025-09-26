using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Game : MonoBehaviour
{
    public Image defeat;
    public Image victory;
    public Image abilities;
    public Image disabled;
    public Text spadesLeft;
    public Text defuseLeft;
    public Text MinesSet;
    public Text FlagsSet;
    public Text MinesDefused;

    private Board board;
    private Cell[,] state;
    private int width;
    private int height;
    private int mineCount;
    private int defuseKitCount;
    private int spadeKitCount;
    private bool gameover;
    private int defuseKitCountCurr;
    private int spadeKitCountCurr;
    private int flagsCountCurr = 0;
    private int minesDefused = 0;
    private bool ability = true;

    private void OnValidate()
    {
        mineCount = Mathf.Clamp(mineCount, 0, width * height);
    }

    private void Awake()
    {
        board = GetComponentInChildren<Board>();
    }

    private void Start()
    {
        SetDifficultyParameters();
        NewGame();
    }

    private void NewGame()
    {
        state = new Cell[width, height];
        gameover = false;
        ability = true;
        defeat.enabled = false;
        victory.enabled = false;
        abilities.enabled = false;
        disabled.enabled = false;
        flagsCountCurr = 0;
        minesDefused = 0;
        defuseKitCountCurr = defuseKitCount;
        spadeKitCountCurr = spadeKitCount;
        spadesLeft.text = $"(S) - {spadeKitCountCurr}";
        defuseLeft.text = $"(D) - {defuseKitCountCurr}";
        MinesSet.text = $"{mineCount}";
        FlagsSet.text = "0";
        MinesDefused.text = "0";

        GenerateCells();
        GenerateMines();
        GenerateNumbers();
        if (!(DifficultyManager.level == 1))
            GenerateDisabler();

        Camera.main.transform.position = new Vector3(width / 2f, height / 2f, -10f);
        board.Draw(state);
    }

    private void SetDifficultyParameters()
    {
        switch (DifficultyManager.level)
        {
            case 1:
                width = 8;
                height = 8;
                mineCount = 8;
                defuseKitCount = 0;
                spadeKitCount = 3;
                break;
            case 2:
                width = 10;
                height = 10;
                mineCount = 18;
                defuseKitCount = 1;
                spadeKitCount = 3;
                break;
            case 3:
                width = 14;
                height = 14;
                mineCount = 45;
                defuseKitCount = 2;
                spadeKitCount = 2;
                break;
            default:
                width = 8;
                height = 8;
                mineCount = 8;
                defuseKitCount = 0;
                spadeKitCount = 3;
                break;
        }
    }

    private void GenerateCells()
    {
        for (int x = 0;  x < width; x++) 
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = new Cell();
                cell.position = new Vector3Int(x, y, 0);
                cell.type = Cell.Type.Empty;
                state[x, y] = cell;
            }
        }
    }

    private void GenerateMines()
    {
        for (int i = 0; i < mineCount; i++)
        {
            int x = Random.Range(0, width);
            int y = Random.Range(0, height);

            while (state[x, y].type == Cell.Type.Mine)
            {
                x++;

                if (x >= width)
                {
                    x = 0;
                    y++;

                    if (y >= height)
                    {
                        y = 0;
                    }
                }
            }

            state[x, y].type = Cell.Type.Mine;
        }
    }

    private void GenerateNumbers()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0;y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    continue;
                }

                cell.number = CountMines(x, y);

                if (cell.number > 0)
                {
                    cell.type = Cell.Type.Number;
                }

                state[x, y] = cell;
            }
        }
    }

    private void GenerateDisabler()
    {
        int x = Random.Range(0, width);
        int y = Random.Range(0, height);
        Cell cell = state[x, y];
        while (cell.type != Cell.Type.Empty)
        {
            x = Random.Range(0, width);
            y = Random.Range(0, height);
            cell = state[x, y];
        }

        cell.type = Cell.Type.AbilityDisabler;
        state[x, y] = cell;
    }

    private int CountMines(int cellX, int cellY)
    {
        int count = 0;

        for (int adjX = -1; adjX < 2; adjX++)
        {
            for (int adjY = -1; adjY < 2; adjY++)
            {
                if (adjX == 0 && adjY == 0)
                {
                    continue;
                }

                int x = cellX + adjX;
                int y = cellY + adjY;

                if (x < 0 || x >= width || y < 0 || y >= height)
                {
                    continue;
                }

                if (state[x, y].type == Cell.Type.Mine)
                {
                    count++;
                }
            }
        }

        return count;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            NewGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene(0);
        }
        else if (!gameover)
        {
            if (Input.GetMouseButtonDown(1) && ability)
            {
                Flag();
            }
            else if (Input.GetMouseButtonDown(0))
            {
                Reveal();
            }
            else if (Input.GetKeyDown(KeyCode.D) && ability && defuseKitCountCurr != 0)
            {
                Defuse();
            }
            else if (Input.GetKeyDown(KeyCode.S) && ability && spadeKitCountCurr != 0)
            {
                Spade();
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                SceneManager.LoadScene(0);
            }
        }
    }

    private void Flag()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        if (cell.type == Cell.Type.Invalid || cell.revealed == true)
        {
            return;
        }

        cell.flagged = !cell.flagged;
        flagsCountCurr += (cell.flagged ? 1 : -1);
        state[cellPosition.x, cellPosition.y] = cell;
        board.Draw(state);
        FlagsSet.text = $"{flagsCountCurr}";
    }

    private void Reveal()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        if (cell.type == Cell.Type.Invalid || cell.revealed == true || cell.flagged == true)
        {
            return;
        }

        switch (cell.type)
        {
            case Cell.Type.Mine: 
                Explode(cell);
                break;

            case Cell.Type.Empty:
                Flood(cell);
                CheckWin();
                break;
            case Cell.Type.AbilityDisabler:
                Disable(cell);
                CheckWin();
                break;

            default:
                cell.revealed = true;
                state[cellPosition.x, cellPosition.y] = cell;
                CheckWin();
                break;
        }

        board.Draw(state);
    }

    private void Defuse()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        if (cell.type == Cell.Type.Invalid || cell.revealed == true || cell.flagged == true)
        {
            return;
        }

        if (cell.type == Cell.Type.Mine)
        {
            MineFlood(cell);
        }
        else
        {
            Reveal();
            Disable(cell);
        }

        defuseKitCountCurr--;
        board.Draw(state);
        defuseLeft.text = $"(D) - {defuseKitCountCurr}";
    }

    private void Spade()
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int cellPosition = board.tilemap.WorldToCell(worldPosition);
        Cell cell = GetCell(cellPosition.x, cellPosition.y);

        if (cell.type == Cell.Type.Invalid || cell.revealed == true || cell.flagged == true)
        {
            return;
        }

        if (cell.type == Cell.Type.Mine)
        {
            cell.difused = true;
            cell.revealed = true;
            state[cell.position.x, cell.position.y] = cell;
            minesDefused++;
        }
        else
        {
            Reveal();
        }

        spadeKitCountCurr--;
        board.Draw(state);
        spadesLeft.text = $"(S) - {spadeKitCountCurr}";
        MinesDefused.text = $"{minesDefused}";
    }

    private void Disable(Cell cell)
    {
        cell.revealed = true;
        state[cell.position.x, cell.position.y] = cell;
        ability = false;
        abilities.enabled = true;
        disabled.enabled = true;
    }

    private void MineFlood(Cell cell)
    {
        if (cell.revealed)
        {
            return;
        }
        if (cell.type == Cell.Type.Empty || cell.type == Cell.Type.Invalid)
        {
            return;
        }
        if (cell.type == Cell.Type.AbilityDisabler)
        {
            ability = false;
        }
        
        if (!cell.difused && cell.type == Cell.Type.Mine)
            minesDefused++;

        cell.difused = true;
        cell.revealed = true;

        if (cell.flagged)
        {
            flagsCountCurr--;
            FlagsSet.text = $"{flagsCountCurr}";
        }

        state[cell.position.x, cell.position.y] = cell;

        if (cell.type == Cell.Type.Mine)
        {
            MineFlood(GetCell(cell.position.x - 1, cell.position.y));
            MineFlood(GetCell(cell.position.x + 1, cell.position.y));
            MineFlood(GetCell(cell.position.x, cell.position.y - 1));
            MineFlood(GetCell(cell.position.x, cell.position.y + 1));
        }

        MinesDefused.text = $"{minesDefused}";
    }

    private void Flood(Cell cell)
    {
        if (cell.revealed)
        {
            return;
        }
        if (cell.type == Cell.Type.Mine || cell.type == Cell.Type.Invalid)
        {
            return;
        }
        if (cell.type == Cell.Type.AbilityDisabler)
        {
            Disable(cell);
        }

        cell.revealed = true;

        if (cell.flagged)
        {
            flagsCountCurr--;
            FlagsSet.text = $"{flagsCountCurr}";
        }

        state[cell.position.x, cell.position.y] = cell;

        if (cell.type == Cell.Type.Empty)
        {
            Flood(GetCell(cell.position.x - 1, cell.position.y));
            Flood(GetCell(cell.position.x + 1, cell.position.y));
            Flood(GetCell(cell.position.x, cell.position.y - 1));
            Flood(GetCell(cell.position.x, cell.position.y + 1));
        }
    }

    private void Explode(Cell cell)
    {
        gameover = true;

        cell.revealed = true;
        cell.exploded = true;
        state[cell.position.x, cell.position.y] = cell;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                cell = state[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    cell.revealed = true;
                    state[x, y] = cell;
                }
            }
        }

        defeat.enabled = true;
    }

    private void CheckWin()
    {
        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.type != Cell.Type.Mine && !cell.revealed)
                {
                    return;
                }
            }
        }

        victory.enabled = true;
        gameover = true;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Cell cell = state[x, y];

                if (cell.type == Cell.Type.Mine)
                {
                    cell.flagged = true;
                    state[x, y] = cell;
                }
            }
        }
    }

    private Cell GetCell(int cellX, int cellY)
    {
        if (IsValid(cellX, cellY))
        {
            return state[cellX, cellY];
        }
        else
        {
            return new Cell();
        }
    }

    private bool IsValid(int cellX, int cellY)
    {
        return cellX >= 0 && cellX < width && cellY >= 0 && cellY < height;
    }
}
