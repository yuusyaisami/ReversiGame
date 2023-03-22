/*
 * 製作者です
 * オセロのプログラムのアルゴリズムです！
 *      マップを生成
 *      先攻を決める(ここで黒か白が決まる）
 *      先攻は黒　後攻は白です！
 *      
 *      ここから繰り返し
 *      もし置ける場所がなかったらスキップ
 *      それかマップが個まで埋まっていたら終了　勝敗を決める
 *      
 *      クリックした場所が有効な場所か判断
 *      有効だった場合　　　　　無効だった場合
 *      色を変える              何もしない
 *      　　↓
 *      相手のターン
 *      繰り返し
 *      
 *      相手のターンファンクション
 *      置けるマスで一番評価値が高いマスを優先的に置く
 *      そのときあまりに多く返しすぎる場合は、優先度を少し下げる
 *      終わり
 *      
 *      そこに駒は置けるか判断する関数
 *      全方向に違う色の駒があったら、
 *      その先に自分の駒があったら
 *      その一個先に駒が置ける状況ならtrueにする
 *      
 *      色を変える関数
 *      置いた場所から、全方向に違う駒があったら
 *      その先に自分の駒があったら、
 *      進んできた違う駒を自分の駒に変える
 *      
 * こんな感じですあとはGUIについてです
 *      ゲームのメインタイトル
 *          ボタン　ゲーム
 *          ボタン　設定
 *          ボタン　URL
 *          //URLは自分のgithubです
 *      終わりです
 *      
 * 設定の項目
 *      音量設定  
 *      
 * 
 */
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NAudio.Wave;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace ReversiGame
{
    public partial class Form1 : Form
    {
        /// <summary>
        /// 0 : null~~1 : black~~2 : white
        /// </summary>
        int[][] ColorPieces = new int[8][];
        int[][] ChengeColorPieces = new int[8][];
        //駒の配列(GUI)100もいらない
        public PictureBox[][] Pieces = new PictureBox[100][];
        //ラインの配列
        public PictureBox[] Lines = new PictureBox[100];
        //その他のControl
        /// <summary>
        /// OtherGUIPictures[0] Green  
        /// OtherGUIPictures[1] DarkGreen
        /// </summary>
        public PictureBox[] OtherGUIPictures = new PictureBox[100];
        public Label MenuGame;
        public Label ExitGame;
        public Label ExitGame1;
        public Label Title;
        public Label Version;
        public Label Game;
        public Label Setting;
        public Label Github;
        public Label ReturnMain;
        public Label SoundText1;
        public System.Windows.Forms.TrackBar SoundBar1;
        public Label SoundTitle1;
        public Label SoundValue1;
        public Label Youlabel;
        public Label Arelabel;
        public Label AttackFirstlabel;
        public Label AttackSecondlabel;
        public Label Passlabel;
        public Label ResultsLabel;
        public Label ResultsLabel1;
        //駒の画像を取得
        Bitmap black = new Bitmap(Properties.Resources.black1);
        Bitmap white = new Bitmap(Properties.Resources.white1);
        Bitmap Transparent = new Bitmap(Properties.Resources.Transparent);

        
        string BaseC = "SoundFile/baseC.wav";
        string BaseE = "SoundFile/baseE.wav";
        string BaseF = "SoundFile/baseF.wav";
        string BaseG = "SoundFile/baseG.wav";

        string Vibes1E = "SoundFile/Vibes1E.wav";
        string Vibes1F = "SoundFile/Vibes1#F.wav";
        string Vibes1G = "SoundFile/Vibes1G.wav";
        string Vibes1A = "SoundFile/Vibes1A.wav";
        string Vibes1B = "SoundFile/Vibes1B.wav";
        string Vibes2C = "SoundFile/Vibes2C.wav";

        string ouou = "SoundFile/ououolu.wav";
        string tantan = "SoundFile/tantan.wav";

        public Form1()
        {
            InitializeComponent();//この下にプログラムを書く
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //駒を配列にしまう 
            for (int i = 0; i < 8; i++)
            {
                Pieces[i] = new PictureBox[100];
                for (int j = 0; j < 8; j++)
                {
                    Pieces[i][j] = (PictureBox)this.Controls["Piece" + i.ToString() + j.ToString()];
                }
            }
            for (int i = 1; i < 19; i++)
            {
                Lines[i] =(PictureBox)this.Controls["Line" + i.ToString()];
            }
            
            OtherGUIPictures[0] = (PictureBox)this.Controls["Green"];
            OtherGUIPictures[1] = (PictureBox)this.Controls["DarkGreen"];
            MenuGame = (Label)this.Controls["MenuBtn"];
            ExitGame = (Label)this.Controls["ExitBtn"];
            ExitGame1 = (Label)this.Controls["ExitBtn1"];
            Title = (Label)this.Controls["TitleText"];
            Version = (Label)this.Controls["VersionText"];
            Game = (Label)this.Controls["GameBtn"];
            Setting = (Label)this.Controls["SettingBtn"];
            Github = (Label)this.Controls["GithubBtn"];
            ReturnMain = (Label)this.Controls["Return"];
            SoundText1 = (Label)this.Controls["SoundText"];
            SoundBar1 = (System.Windows.Forms.TrackBar)this.Controls["SoundBar"];
            SoundTitle1 = (Label)this.Controls["SettingTitle"];
            SoundValue1 = (Label)this.Controls["SoundValue"];
            Youlabel = (Label)this.Controls["YouText"];
            Arelabel = (Label)this.Controls["AreText"];
            AttackFirstlabel = (Label)this.Controls["AttackFirstText"];
            AttackSecondlabel = (Label)this.Controls["AttackSecondText"];
            Passlabel = (Label)this.Controls["PassText"];
            ResultsLabel = (Label)this.Controls["ResultsText"];
            ResultsLabel1 = (Label)this.Controls["ResultsText1"];

            for (int i = 0; i < 8; i++)
            {
                ColorPieces[i] = new int[8];
                for (int j = 0; j < 8; j++)
                {
                    ColorPieces[i][j] = 0;
                }
            }
            for (int i = 0; i < 8; i++)
            {
                ChengeColorPieces[i] = new int[8];
                for (int j = 0; j < 8; j++)
                {
                    ChengeColorPieces[i][j] = 0;
                }
            }
        }
        private void FinishGame()
        {//ゲームが終了する
            ResultsBlack = ResultsWhite = 0;
            bool check = false;
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(ColorPieces[i][j] == 0 )
                    {
                        check = true;
                    }
                    
                }
            }
            if (check == false)
            {
                for (int i = 0; i < 8; i++)
                {
                    for (int j = 0; j < 8; j++)
                    {
                        if (ColorPieces[i][j] == 1)
                        {
                            ResultsBlack++;
                        }
                        if (ColorPieces[i][j] == 2)
                        {
                            ResultsWhite++;
                        }
                    }
                }
                Animation3 = true;
            }
        }
        /// <summary>
        /// 座標の位置に駒を置きます
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="color"></param>
        private void PutaPiece(int x,int y,int color)
        {

        }
        /*
         * アルゴリズム
         *  最初に全方向に敵の駒があるか確認します
         *  そこに敵の駒があった場合、数値を保存する
         *  何マスあるかも確認
         *  
         *  保存した方向の先に自分の駒があるか確認する。
         *  仕組み
         *  繰り返す
         *  一個先の座標にする
         *  そこに自分の駒がある確認
         *  あったらonをtrueにするそしてbreak;
         *  敵のがあったら、繰り返す
         *  何もなかったらまたは範囲外だったらbreak;
         *  繰り返す
         */

        /// <summary>
        /// クリックした場所に駒を置けるか確認します。colorは自分自身の色
        /// </summary>
        private void CheckPlacePiece(int x, int y, int color)
        {
            bool on = false;
            int enemycolor;
            if (color == 1) { enemycolor = 2; }
            else { enemycolor = 1; }
            if(ColorPieces[y][x] == 1 || ColorPieces[y][x] == 2)
            {
                return;
            }
            for(int i = y - 1; i < y + 2; i++)
            {
                for(int j = x - 1; j < x + 2; j++)
                {
                    if (CheckArray(j, i, ColorPieces.Length, ColorPieces.Length))
                    {
                        if (ColorPieces[i][j] == enemycolor)
                        {
                            //左↑　右↑ ↑
                            if (i == y - 1 && j == x - 1)
                            {
                                for (int b = i, a = j; ; b--, a--)
                                {
                                    if (CheckArray(a, b, ColorPieces.Length, ColorPieces.Length))
                                    {
                                        if (ColorPieces[b][a] == color)
                                        {
                                            CanChenge = true;
                                            DebugText.Text += "左上";
                                            for (; ; b++, a++)
                                            {
                                                ChengeColorPieces[b][a] = color;
                                                if (b == i && a == j)
                                                {
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                        else if (ColorPieces[b][a] == 0) { break; }
                                    }
                                    else { break; }
                                }
                            }
                            if (i == y - 1 && j == x)
                            {
                                for (int b = i, a = j; ; b--)
                                {
                                    if (CheckArray(a, b, ColorPieces.Length, ColorPieces.Length))
                                    {
                                        
                                        if (ColorPieces[b][a] == color)
                                        {
                                            DebugText.Text += "上";
                                            CanChenge = true;
                                            for (; ; b++)
                                            {
                                                ChengeColorPieces[b][a] = color;
                                                if(b == i)
                                                {
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                        else if (ColorPieces[b][a] == 0) { break; }
                                    }
                                    else { break; }
                                }
                            }
                            if (i == y - 1 && j == x + 1)
                            {
                                for (int b = i, a = j; ; b--, a++)
                                {
                                    if (CheckArray(a, b, ColorPieces.Length, ColorPieces.Length))
                                    {
                                        if (ColorPieces[b][a] == color)
                                        {
                                            CanChenge = true;
                                            DebugText.Text += "右上";
                                            for (; ; b++, a--)
                                            {
                                                ChengeColorPieces[b][a] = color;
                                                if (b == i && a == j)
                                                {
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                        else if (ColorPieces[b][a] == 0) { break; }
                                    }
                                    else { break; }
                                }
                            }
                            //左　右
                            if (i == y && j == x - 1)
                            {
                                for (int b = i, a = j; ; a--)
                                {
                                    if (CheckArray(a, b, ColorPieces.Length, ColorPieces.Length))
                                    {
                                        if (ColorPieces[b][a] == color)
                                        {
                                            CanChenge = true;
                                            DebugText.Text += "左";
                                            for (; ; a++)
                                            {
                                                ChengeColorPieces[b][a] = color;
                                                if (b == i && a == j)
                                                {
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                        else if (ColorPieces[b][a] == 0) { break; }
                                    }
                                    else { break; }
                                }
                            }
                            if (i == y && j == x + 1)
                            {
                                for (int b = i, a = j; ; a++)
                                {
                                    if (CheckArray(a, b, ColorPieces.Length, ColorPieces.Length))
                                    {
                                        if (ColorPieces[b][a] == color)
                                        {
                                            CanChenge = true;
                                            DebugText.Text += "右";
                                            for (; ; a--)
                                            {
                                                ChengeColorPieces[b][a] = color;
                                                if (b == i && a == j)
                                                {
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                        else if (ColorPieces[b][a] == 0) { break; }
                                    }
                                    else { break; }
                                }
                            }
                            //左↓　右↓
                            if (i == y + 1 && j == x - 1)
                            {
                                for (int b = i, a = j; ; b++, a--)
                                {
                                    if (CheckArray(a, b, ColorPieces.Length, ColorPieces.Length))
                                    {
                                        if (ColorPieces[b][a] == color)
                                        {
                                            CanChenge = true;
                                            DebugText.Text += "左下";
                                            for (; ; b--, a++)
                                            {
                                                ChengeColorPieces[b][a] = color;
                                                if (b == i && a == j)
                                                {
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                        else if (ColorPieces[b][a] == 0) { break; }
                                    }
                                    else { break; }
                                }
                            }
                            if (i == y + 1 && j == x)
                            {
                                for (int b = i, a = j; ; b++)
                                {
                                    if (CheckArray(a, b, ColorPieces.Length, ColorPieces.Length))
                                    {
                                        if (ColorPieces[b][a] == color)
                                        {
                                            CanChenge = true;
                                            DebugText.Text += "下";
                                            for (; ; b--)
                                            {
                                                ChengeColorPieces[b][a] = color;
                                                if (b == i && a == j)
                                                {
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                        else if (ColorPieces[b][a] == 0) { break; }
                                    }
                                    else { break; }
                                }
                            }
                            if (i == y + 1 && j == x + 1)
                            {
                                for (int b = i, a = j; ; b++, a++)
                                {
                                    if (CheckArray(a, b, ColorPieces.Length, ColorPieces.Length))
                                    {
                                        if (ColorPieces[b][a] == color)
                                        {
                                            CanChenge = true;
                                            DebugText.Text += "左下";
                                            for (; ; b--, a--)
                                            {
                                                ChengeColorPieces[b][a] = color;
                                                if (b == i && a == j)
                                                {
                                                    break;
                                                }
                                            }
                                            break;
                                        }
                                        else if (ColorPieces[b][a] == 0) { break; }
                                    }
                                    else { break; }
                                }
                            }

                        }
                    }
                }
            }

            if (CanChenge == true)
            {
                ChengeColorPieces[y][x] = color;
            }
            
        }
        private void ChengeColor()

        {
            
            
            for (int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    if(ChengeColorPieces[i][j] == 1)
                    {

                        ColorPieces[i][j] = 1;
                        var old = Pieces[i][j].Image;
                        Pieces[i][j].Image =  new Bitmap(Properties.Resources.black1);
                        old.Dispose();
                    }
                    else if (ChengeColorPieces[i][j] == 2)
                    {
                        ColorPieces[i][j] = 2;
                        var old = Pieces[i][j].Image;
                        Pieces[i][j].Image =  new Bitmap(Properties.Resources.white1);
                        old.Dispose();
                       
                    }
                }
            }
        }
        
        /// <summary>
        /// 配列が範囲内かどうかを調べる関数
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="array"></param>
        /// <returns></returns>
        private bool CheckArray(int x,int y,int Lengthx, int Lengthy)
        {
            if(x < 0 || y < 0)
            {
                return false;
            }
            if(Lengthx <= x || Lengthy <= y)
            {
                return false;
            }
            return true;
            
        }
        /// <summary>
        /// 現在の盤面で自分の駒が置けるか確認します
        /// </summary>
        private void CheckPass(int color)
        {
            
                for(int i =0; i < 8; i++)
                {
                    for(int j = 0; j < 8; j++)
                    {
                        CheckPlacePiece(i,j,color);

                    }
                }
            if (CanChenge == false)
            {
                Animation2 = true;
                
            }
            CanChenge = false;
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {

                    ChengeColorPieces[i][j] = 0;

                }
            }

        }
        
        private void CheckClick()
        {//関数の中にある関数　クリックした場所のマップ座標を記憶します
            Point p = this.PointToClient(Control.MousePosition);
            int LocationX, LocationY;
            for(int i = 0; i < 8; i++)
            {
                for(int j = 0; j < 8; j++)
                {
                    LocationX = Pieces[i][j].Location.X;
                    if(LocationX + 57 > p.X && LocationX - 13 < p.X)
                    {
                        LocationY = Pieces[i][j].Location.Y;
                        if (LocationY + 56 > p.Y && LocationY - 6 < p.Y)
                        {
                            PX = j;
                            PY = i;
                        }
                    }
                }
            }
        }
        /// <summary>
        /// Pieces配列の透明化と、ColorPieces配列の初期化
        /// </summary>
        private void InitializePicture()
        {//画像を初期化する
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    var old = Pieces[i][j].Image;
                    Pieces[i][j].Image = new Bitmap(Transparent);
                    old.Dispose();

                    ColorPieces[i][j] = 0;
                }
            }
        }
        private void InitializeChengeColorPicture()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    ColorPieces[i][j] = 0;
                }
            }
        }
        private async void PlayAudioAsync(string filePath)
        {
            using (var audioFile = new AudioFileReader(filePath))
            using (var outputDevice = new WaveOutEvent())
            {
                audioFile.Volume = audioVolume / 4f;
                outputDevice.Init(audioFile);
                outputDevice.Play();
                await Task.Delay(audioFile.TotalTime);
            }
        }


        //------------------------------------------------------------------------------------------------------------------------------------------
        //Attack first, then attack later　先攻後攻
        /// <summary>
        /// 先攻後攻を決める
        /// </summary>
        bool Animation1 = false;
        bool Animation2 = false;
        bool Animation3 = false;
        int count = 0, RandomAnimation = 0,ResultsBlack,ResultsWhite,PassCount;
        bool PlayerControl = false,COMControl = false,CanChenge = false,Pass = false;
        int PX = -1, PY = -1;
        private void timer1_Tick(object sender, EventArgs e)
        {//先攻後攻を決めるアニメーション
            if (Animation1)
            {
                count++;
                if(count == 15)
                {
                    PlayAudioAsync(Vibes1E);
                    Youlabel.Visible = true;
                }
                if(count == 30)
                {
                    PlayAudioAsync(Vibes1F);
                    Arelabel.Visible = true;
                }
                if(count == 45)
                {
                    PlayAudioAsync(Vibes1G);
                    RandomAnimation = new Random().Next(1, 2);
                    if(RandomAnimation == 1)
                    {
                        AttackFirstlabel.Visible = true;
                    }
                    else 
                    {
                        AttackSecondlabel.Visible = true;
                    }
                }
                if(count == 70)
                {
                    if(RandomAnimation == 1)
                    {
                        COMControl = false;
                        PlayerControl = true;
                    }
                    else
                    {
                        PlayerControl = false;
                        COMControl = true;
                    }
                    AttackSecondlabel.Visible = AttackFirstlabel.Visible = Arelabel.Visible = Youlabel.Visible = Animation1 = false;
                    Pass = true;
                    count = 0;
                    Pieces[3][3].Image = new Bitmap(Properties.Resources.white1);
                    Pieces[4][3].Image = new Bitmap(Properties.Resources.black1);
                    Pieces[3][4].Image = new Bitmap(Properties.Resources.black1);
                    Pieces[4][4].Image = new Bitmap(Properties.Resources.white1);

                    ColorPieces[3][3] = 2;
                    ColorPieces[4][3] = 1;
                    ColorPieces[3][4] = 1;
                    ColorPieces[4][4] = 2;
                    
                }
                
            }
            else if (Animation2)
            {
                count++;
                if(count == 10)
                {
                    Passlabel.Visible = true;
                    PlayAudioAsync(ouou);
                }
                if(count == 40)
                {
                    Passlabel.Visible = false;
                    if (PlayerControl)
                    {

                        PlayerControl = false;
                        COMControl = true;
                        count = 0;
                    }
                    else
                    {
                        COMControl = false;
                        PlayerControl = true;
                        count = 0;
                    }
                    Animation2 = false;
                }
                
            }
            else if (Animation3)
            {
                count++;
                if(count == 20)
                {
                    PlayAudioAsync(Vibes1A);
                    ResultsLabel.Text = "Black : " + ResultsBlack + " | " + "White : " + ResultsWhite;
                    ResultsLabel.Visible = true;
                }
                if(count == 40)
                {
                    PlayAudioAsync(Vibes1B);
                    PlayAudioAsync(Vibes2C);
                    PlayAudioAsync(Vibes1F);
                    if (ResultsBlack > ResultsWhite)
                        ResultsLabel1.Text = "Winner Black!";
                    else if (ResultsWhite > ResultsBlack)
                        ResultsLabel1.Text = "Winner White";
                    else
                    {
                        ResultsLabel1.Text = "Draw";
                    }
                    ResultsLabel1.Visible = true;
                    
                }
                if((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left && count > 40 )
                {
                    AllCloseScene();
                    StartMenuScene();
                    Animation3 = false;
                    count = 0;
                }

            }
            //プレイヤーが操作する
            if (PlayerControl)
            {
                if (Pass)
                {
                    FinishGame();
                    if (Animation3 == false)
                        CheckPass(1);
                }
                    
                Pass = false;
                if ((Control.MouseButtons & MouseButtons.Left) == MouseButtons.Left && Animation2 == false)
                {
                    CheckClick();
                    CheckPlacePiece(PX, PY, 1);
                    if (CanChenge)
                    {
                        PlayAudioAsync(tantan);
                        ChengeColor();
                        PlayerControl = false;
                        COMControl = true;
                        CanChenge = false;
                        Pass = true;
                    }
                    
                }


            }
            //コンピューターが操作する
            else if (COMControl)
            {
                if (Pass)
                {
                    FinishGame();
                    if(Animation3 == false)
                    CheckPass(2);
                }
                Pass = false;
                for (; Animation2 != true; )
                {
                    
                    int randomx = new Random().Next();
                    int randomy = new Random().Next(0,8);
                    CheckPlacePiece(randomx % 8, randomy, 2);
                    DebugText.Text = randomx.ToString() + randomy;
                    if (CanChenge)
                    {
                        PlayAudioAsync(tantan);
                        ChengeColor();
                        PlayerControl = true;
                        COMControl = false;
                        CanChenge = false;
                        Pass = true;
                        break;
                    }
                }
                
            }
        }
        private void Piece14_Click(object sender, EventArgs e)
        {

        }

        private void Piece13_Click(object sender, EventArgs e)
        {

        }

        private void Piece12_Click(object sender, EventArgs e)
        {

        }

        private void Piece11_Click(object sender, EventArgs e)
        {

        }

        private void Piece10_Click(object sender, EventArgs e)
        {

        }

        private void Piece15_Click(object sender, EventArgs e)
        {

        }

        private void Piece16_Click(object sender, EventArgs e)
        {

        }

        private void Piece17_Click(object sender, EventArgs e)
        {

        }

        private void Form1_MouseDown(object sender, EventArgs e)
        {

        }

        private void Form1_MouseClick(object sender, MouseEventArgs e)
        {
            
        }

        private void MenuBtn_Click(object sender, EventArgs e)
        {

        }

        private void ExitBtn_Click(object sender, EventArgs e)
        {

        }

        private void GameBtn_Click(object sender, EventArgs e)
        {
            InitializePicture();
            AllCloseScene();
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Pieces[i][j].Visible = true;
                }
            }
            for (int i = 1; i < 19; i++)
            {
                Lines[i].Visible = true;
            }
            for (int i = 0; i < 2; i++)
            {
                OtherGUIPictures[i].Visible = true;
            }
            MenuGame.Visible = ExitGame.Visible = true;
            Animation1 = true;
        }
        
        private void SettingBtn_Click(object sender, EventArgs e)
        {
            SettingScene();
        }

        private void GithubBtn_Click(object sender, EventArgs e)
        {
            OpenUrl("https://github.com/yuusyaisami");
        }
        private void Return_Click(object sender, EventArgs e)
        {
            StartMenuScene();
        }
        int audioVolume = 1;
        private void SoundBar_Scroll(object sender, EventArgs e)
        {
            int value;
            value = SoundBar1.Value * 20;
            SoundValue1.Text = value.ToString() + "%";
            audioVolume = SoundBar.Value;
        }
        /// <summary>
        /// 1/30で再生される
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
       

        private void SettingInMouse(object sender, EventArgs e)
        {
            PlayAudioAsync(BaseE);
            SettingBtn.BackColor = Color.FromArgb(97, 128, 32);

        }

        private void SettingLeaveMouse(object sender, EventArgs e)
        {
            SettingBtn.BackColor = Color.OliveDrab;
        }

        private void GameBtnInMouse(object sender, EventArgs e)
        {
            PlayAudioAsync(BaseC);
            GameBtn.BackColor = Color.FromArgb(97, 128, 32);
        }

        private void GameBtnLeaveMouse(object sender, EventArgs e)
        {
            GameBtn.BackColor = Color.OliveDrab;
        }

        private void GithubInMouse(object sender, EventArgs e)
        {
            PlayAudioAsync(BaseF);
            GithubBtn.BackColor = Color.FromArgb(97, 128, 32);
        }

        private void ClickTitle(object sender, EventArgs e)
        {
            PlayAudioAsync(ouou);
        }

        private void GithubLeaveMouse(object sender, EventArgs e)
        {
            GithubBtn.BackColor = Color.OliveDrab;
        }

        private void ExitInMouse(object sender, EventArgs e)
        {
            PlayAudioAsync(BaseG);
            ExitBtn1.BackColor = Color.FromArgb(97, 128, 32);
        }

        private void ExitLeaveMouse(object sender, EventArgs e)
        {
            ExitBtn1.BackColor = Color.OliveDrab;
        }







        public void StartMenuScene()
        {
            AllCloseScene();
            ExitGame1.Visible = true;
            Title.Visible = Version.Visible = Game.Visible = true;
            Setting.Visible = Github.Visible =true;
        }
        public void GameScene()
        {

        }
        public void SettingScene()
        {
            AllCloseScene();
            SoundText1.Visible = SoundBar1.Visible = SoundTitle1.Visible = ReturnMain.Visible = SoundValue1.Visible = true;
        }
        public void AllCloseScene()
        {
            for (int i = 0; i < 8; i++)
            {
                for (int j = 0; j < 8; j++)
                {
                    Pieces[i][j].Visible = false;
                }
            }
            for (int i = 1; i < 19; i++)
            {
                Lines[i].Visible = false;
            }
            for (int i = 0; i < 2; i++)
            {
                OtherGUIPictures[i].Visible = false;
            }
            MenuGame.Visible = ExitGame.Visible = ExitGame1.Visible = false;
            Title.Visible = Version.Visible = Game.Visible = false;
            Setting.Visible = Github.Visible = ReturnMain.Visible = false;
            SoundText1.Visible = SoundBar1.Visible = SoundTitle1.Visible = SoundValue1.Visible = false;
            Youlabel.Visible = Arelabel.Visible = AttackFirstlabel.Visible = AttackSecondlabel.Visible = Passlabel.Visible = false;
            ResultsLabel.Visible = ResultsLabel1.Visible = false;
    }
        private Process OpenUrl(string url)
        {
            ProcessStartInfo pi = new ProcessStartInfo()
            {
                FileName = url,
                UseShellExecute = true,
            };

            return Process.Start(pi);
        }

    }
   
}
