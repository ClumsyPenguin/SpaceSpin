using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Nez;
using Nez.AI.Pathfinding;
using Nez.BitmapFonts;
using SpaceSpin.Enemies;
using SpaceSpin.Player;
using SpaceSpin.Shared;
using SpaceSpin.Shared.Components;
using SpaceSpin.Shared.Game;
using SpaceSpin.Shared.UI;

namespace SpaceSpin.Scenes
{
    public class GameScene : Scene
    {
        public UnweightedGraph<Point> Graph;
        public float CellSize = 40f;

        private const float IntermissionTime = 5f;
        private float _waveCountdownTimer;
        private GameState _gameState;
        private int _currentWave = 0;

        private readonly Dictionary<int, Wave> _waves = new();

        private int _cols;
        private int _rows;

        public GameScene()
        {
            ClearColor = Color.Black;
        }
        
        public override void Initialize()
        {
            base.Initialize();
            var bloom = AddPostProcessor(new BloomPostProcessor(0));
            bloom.Settings = new BloomSettings(0.1f, 1.5f, 2f, 1f, 1, 0);
            
            ClearColor = Color.CornflowerBlue;

            // --- Wave Definitions ---
            _waves[1] = new Wave { SpawnPositions = [new Vector2(100, 100), new Vector2(800, 600)] };
            _waves[2] = new Wave { SpawnPositions = [new Vector2(100, 600), new Vector2(800, 100)] };
            // --- End Wave Definitions ---

            CreateGridGraph();
            CreatePlayer();

            var uiEntity = CreateEntity("ui");
            uiEntity.AddComponent(new WaveUI(this));

            _gameState = GameState.Intermission;
            _waveCountdownTimer = IntermissionTime;
        }
        
        public override void Update()
        {
            base.Update();

            if (_currentWave is 0)
            {
                _currentWave++;
                SpawnWave(_currentWave);
                _waveCountdownTimer = IntermissionTime;
            }
            else
            {
                _waveCountdownTimer -= Time.DeltaTime;

                if (_waveCountdownTimer <= 0)
                {
                    _currentWave++;
                    SpawnWave(_currentWave);
                    _waveCountdownTimer = IntermissionTime;
                }
            }

            _gameState = FindEntitiesWithTag(ComponentRegister.BaseEnemyTag).Count > 0 
                ? GameState.WaveActive 
                : GameState.Intermission;
        }

        public GameState GetGameState() => _gameState;
        public float GetWaveCountdownTimer() => _waveCountdownTimer;
        public int GetCurrentWave() => _currentWave;

        private void CreateGridGraph()
        {
            if (CellSize <= 0f)
                CellSize = 40f;

            _cols = (int)(Screen.Width / CellSize);
            _rows = (int)(Screen.Height / CellSize);

            if (_cols <= 0) _cols = 1;
            if (_rows <= 0) _rows = 1;

            Graph = new UnweightedGraph<Point>();

            for (var y = 0; y < _rows; y++)
            {
                for (var x = 0; x < _cols; x++)
                {
                    var node = new Point(x, y);
                    var neighbors = new List<Point>();

                    if (x > 0 && y > 0) neighbors.Add(new Point(x - 1, y - 1)); // NW
                    if (x < _cols - 1 && y > 0) neighbors.Add(new Point(x + 1, y - 1)); // NE
                    if (x > 0 && y < _rows - 1) neighbors.Add(new Point(x - 1, y + 1)); // SW
                    if (x < _cols - 1 && y < _rows - 1) neighbors.Add(new Point(x + 1, y + 1)); // SE

                    if (x > 0) neighbors.Add(new Point(x - 1, y)); // W
                    if (x < _cols - 1) neighbors.Add(new Point(x + 1, y)); // E
                    if (y > 0) neighbors.Add(new Point(x, y - 1)); // N
                    if (y < _rows - 1) neighbors.Add(new Point(x, y + 1)); // S

                    Graph.AddEdgesForNode(node, neighbors.ToArray());
                }
            }
        }
        
        private void CreatePlayer()
        {
            var center = new Vector2(Screen.Width / 2f, Screen.Height / 2f);
            var font = Core.Content.Load<BitmapFont>(ComponentRegister.DefaultFont);

            var hexEntity = CreateEntity(ComponentRegister.Player);
            hexEntity.Position = center;
            hexEntity.AddComponent(new HealthComponent(500));

            hexEntity.AddComponent(new CircleCollider(50f));
            hexEntity.AddComponent(new Hexagon(50f, font));
        }

        
        private void SpawnWave(int waveNumber)
        {
            if (_waves.TryGetValue(waveNumber, out var wave))
            {
                foreach (var position in wave.SpawnPositions)
                {
                    var enemyEntity = CreateEntity(ComponentRegister.BaseEnemy);
                    enemyEntity.Tag = ComponentRegister.BaseEnemyTag;
                    enemyEntity.Position = position;
                    enemyEntity.AddComponent(new Enemy());
                }
            }
            else
            {
                // Handle what happens when there are no more waves (e.g., you win!)
            }
        }
    }
}