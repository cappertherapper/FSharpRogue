module Game
open System

///<summary>Funktion der fjerner index x fra en liste lst</summary>
///<param name = "x">Heltal, index i listen, starter ved 0</param>
///<param name = "lst">En liste</param>
let rec remove x lst =
  match x, lst with
  | 0, n::xs -> xs
  | i, n::xs -> n :: (remove (x-1) xs)
  | i,[] -> failwith "Index out of bounds"
///<returns >Returner en liste med et element mindre</returns>

///<summary> Funktionen laver tilfældige heltal fra 0 til en int </summary>
///<param name = "int"> heltal </param>
let rand : int -> int = let rnd = System.Random()
                        in fun n -> rnd.Next(0,n)
///<returns >Returnerer et tilfældigt heltal mellem 0 og int</returns>

//11g0
type Color = ConsoleColor
type Canvas  (rows:int,cols:int) =
  let C = Array2D.create rows cols (' ',Color.Black,Color.Black)
  ///<summary> Funktion der overskriver et felt (x,y) i et 2D Array med 
  /// nye værdier for; karakter, forgrundsfarve og baggrundsfarve. </summary>
  ///<param name = "x">heltal, x koordinat</param>
  ///<param name = "y">heltal, y koordinat</param>
  ///<param name = "c">karakter</param>
  ///<param name = "fg">Farve, farver kun karakteren</param>
  ///<param name = "bg">Farve, farver feltet</param>
  member this.Set (x:int,y:int,c:char,fg:Color,bg:Color) =
    C.[x,y] <- (c,fg,bg)
  ///<returns >Intet returneres, men et felt i Arrayet C opdateres.</returns>
  
  ///<summary>Funktion der viser et Array i terminalen</summary>
  member this.Show =
    for i=0 to rows-1 do
      for j=0 to cols-1 do
        let (c,fg,bg) = C.[i,j]
        Console.ForegroundColor <- fg
        Console.BackgroundColor <- bg
        printf "%c" c
      System.Console.ResetColor()
      printf "\n"
  ///<returns >Der returneres et Array af rows*cols størrelse, hvor konsollen
  /// farves med de farver og characters som Arrayet har som gemte 
  /// værdier</returns>
  member this.Rows with get() = rows
  member this.Cols with  get() = cols
  member this.BGO (x,y) =
    let (a,b,c) = C.[x,y]
    (c)

//11g1
[<AbstractClass>]
type Entity() =
  abstract member RenderOn : Canvas -> unit

type Player(x,y,c,fg,bg) =
  inherit Entity()
  let mutable hp = 10
  let mutable loc = (x,y)
  let mutable ploc = (x,y)
  let mutable win = false
  let mutable onfire = 0
  ///<summary>RenderOn er blot en funktion som</summary>
  ///<param name = "C">Tager et Canvas som argument</param>
  override this.RenderOn (C:Canvas) = C.Set (fst loc,snd loc,c,fg,bg)
  ///<returns >Opdaterer et felt i Canvas med spillerens karakter,
  /// forgrundsfarve og baggrundsfarve.</returns>
  member this.Hitpoints with get() = hp
  member this.Loc with get() = loc
  member this.PLoc with get() = ploc
  member this.IsDead with get() = hp < 0
  member this.Damage (dmg:int) = hp <- (hp - dmg)
  member this.Heal (h:int) = hp <- (hp + h)
  ///<summary> Opdaterer spillerens placering på Canvas og gemmer tidligere
  /// placering.</summary>
  ///<param name = "g"> heltal, x koordinat</param>
  ///<param name = "j"> heltal, y koordinat</param>
  member this.MoveTo (g:int,j:int) =
    ploc <- loc
    loc <- (g,j)
  ///<returns >Opdaterer placering og gemmer tidl. placering</returns>
  member val Win = false with get,set
  member this.OnFire (f:int) = onfire <- onfire + f
  member this.FireTime = onfire
  member this.IsOnFire =
    if onfire > 0 then true
    else false

[<AbstractClass>]
type Item(x,y)=
  inherit Entity()
  ///<summary> Funktionen har til formål at benytte spiller-funktioner
  /// såfremt den givne item skader eller healer spilleren, eller teleporterer
  /// ham.</summary>
  abstract member InteractWith : Player -> bool
  ///<returns >Returnerer en bool der giver udtryk for om item fortsat skal
  /// være på mappet</returns>
  ///<summary>Funktioen skal fortælle om spilleren kan være på samme felt
  /// som en item</summary>
  abstract member FullyOccupy : bool
  ///<returns >Returnerer en bool, hvis den er sand kan spilleren ikke
  /// rykke til det felt.</returns>
  abstract member Loc : (int*int)

type Wall (x,y) =
  inherit Item(x,y)
  override this.RenderOn (C:Canvas) =
    C.Set (x,y,'#', Color.DarkYellow,Color.DarkYellow)
  override this.InteractWith (P:Player) = true
  override this.FullyOccupy = true
  override this.Loc = (x,y)

  type Wall2 (x,y) =
    inherit Item(x,y)
    override this.RenderOn (C:Canvas) =
      C.Set (x,y,'#', Color.Yellow,Color.Yellow)
    override this.InteractWith (P:Player) = true
    override this.FullyOccupy = true
    override this.Loc = (x,y)

type Water (x,y) =
  inherit Item(x,y)
  override this.RenderOn (C:Canvas) = C.Set (x,y,' ',Color.Blue,Color.Blue)
  ///<summary>Funktion som healer spilleren hvis de er på samme felt</summary>
  ///<param name = "P"> Spiller </param>
  override this.InteractWith (P:Player) =
    if P.Hitpoints > 19 then true
    elif P.Hitpoints = 19 then
      P.Heal 1
      P.OnFire (-P.FireTime)
      true
    else
      P.Heal 2
      P.OnFire (-P.FireTime)
      true
  ///<returns >Healer spilleren op til et maksimum af 20 hitpoints.
  /// Returnerer true, da item ikke forsvinder efter interaktion.</returns>
  override this.FullyOccupy = false
  override this.Loc = (x,y)

type Fire (x,y) =
  inherit Item(x,y)
  let mutable counter = 5
  override this.RenderOn (C:Canvas) = C.Set (x,y,' ',Color.Black,Color.Red)
  ///<summary>Udvidelse: Funktionen aktiverer en tilstand hos spilleren hvor
  /// der er sat ild til dem og de mister 1 hp i de efterfølgende
  /// 5 runder</summary>
  ///<param name = "P"> Spiller </param>
  override this.InteractWith (P:Player) =
    if counter > 0 then
      counter <- (counter - 1)
      P.OnFire (5-P.FireTime)
      true
    else
      false
  ///<returns >Indsat counter, efter fem turer stopper ilden 
  /// med at brænde</returns>
  override this.FullyOccupy = false
  override this.Loc = (x,y)

type FleshEatingPlant (x,y) =
  inherit Item(x,y)
  override this.RenderOn (C:Canvas) = 
    C.Set (x,y,'&',Color.Black,Color.DarkGreen)
  override this.InteractWith (P:Player) =
    P.Damage 5
    true
  override this.FullyOccupy = true
  override this.Loc = (x,y)

type Teleport (x,y) =
  inherit Item(x,y)
  override this.RenderOn (C:Canvas) = C.Set (x,y,'∆',Color.Black,Color.Cyan)
  override this.InteractWith (P:Player) = true
  override this.FullyOccupy = false
  override this.Loc = (x,y)

type Exit (x,y) =
  inherit Item(x,y)
  override this.RenderOn (C:Canvas) = C.Set (x,y,'∑',Color.Black,Color.White)
  ///<summary>Spiller slutter når spilleren er på samme koordinat som
  /// Exit med mindst 5 hitpoints.</summary>
  ///<param name = "P">Spiller</param>
  override this.InteractWith (P:Player) =
    if P.Hitpoints < 5 then true
    else
      P.Win <- true
      true
  ///<returns >Opdaterer spillerens muterbare tilstand "Win" til
  ///sand hvis hitpoints er over 5, ellers opdateres tilstanden ikke</returns>
  override this.FullyOccupy = false
  override this.Loc = (x,y)

///<summary>Fortæller om spillerens lokation på Canvas er udenfor mappet 
/// eller på en item der fylder hele sit felt.</summary>
///<param name = "P">Spiller</param>
///<param name = "C">Canvas</param>
let NoAreas (P:Player) (C:Canvas)=
  (fst P.Loc < 0) || (snd P.Loc < 0) || (fst P.Loc >= C.Rows) || (snd P.Loc >= C.Cols)
let NoNoAreas (P:Player) (C:Canvas) =
  (C.BGO P.Loc = Color.Yellow) || (C.BGO P.Loc = Color.DarkYellow) || (C.BGO P.Loc = Color.DarkGreen)
///<returns >Returnerer sand eller falsk afhængig af spillerens lok.</returns>


//11g2
type World (C:Canvas) =
  let P = Player (0,1,'@',Color.Magenta,Color.Black)
  let mutable ItemList : Item list = []
  ///<summary>Funktion der tilføjer en Item til en muterbar liste
  /// af Items</summary>
  ///<param name = "I"> heltal </param>
  member this.AddItem (I:Item) =
    ItemList <- ItemList @ [I]
  ///<returns >Opdaterer en liste til at indholde argumentet I</returns>
  ///<summary>En funktion der har til formål at vise en map og en spiller
  /// som kan bevæge sig rundt på mappet og inteagere med Items</summary>
  member this.Play()=
    while (not P.IsDead) && not P.Win do
      Console.Clear()
      List.iter (fun (x:Item) -> x.RenderOn C) ItemList
      P.RenderOn C
      C.Show
      printfn "HP: %i" P.Hitpoints
      if P.IsOnFire then printfn "%s" "YOU ARE ON FIRE!!!"
      match Console.ReadKey(true).Key with
      |ConsoleKey.R -> P.MoveTo (0,1)
      |ConsoleKey.W ->
        match (P.Loc, P.PLoc) with
        | (x1,y1), (x2,y2) when y1 > y2 && x1 = x2 ->
          P.MoveTo (fst P.Loc, (snd P.Loc)-2)
        | (x1,y1), (x2,y2) when y1 < y2 && x1 = x2 ->
          P.MoveTo (fst P.Loc, (snd P.Loc)+2)
        | (x1,y1), (x2,y2) when x1 > x2 && y1 = y2 ->
          P.MoveTo ((fst P.Loc)-2, snd P.Loc)
        | (x1,y1), (x2,y2) when x1 < x2 && y1 = y2 ->
          P.MoveTo ((fst P.Loc)+2, snd P.Loc)
        | (x1,y1), (x2,y2) when x1 = x2 && y1 = y2 ->
          P.MoveTo (fst P.Loc, snd P.Loc)
        | _ -> P.MoveTo (fst P.Loc, (snd P.Loc))
      |ConsoleKey.UpArrow -> P.MoveTo ((fst P.Loc)-1, snd P.Loc)
      |ConsoleKey.DownArrow -> P.MoveTo (fst P.Loc+1, (snd P.Loc))
      |ConsoleKey.LeftArrow -> P.MoveTo ((fst P.Loc), snd P.Loc-1)
      |ConsoleKey.RightArrow -> P.MoveTo ((fst P.Loc), snd P.Loc+1)
      |_ -> P.MoveTo P.Loc
      if P.IsOnFire then
        P.Damage 1
        P.OnFire -1
      C.Set (fst P.PLoc,snd P.PLoc,' ',Color.Black,Color.Black)
      if NoAreas P C then
        P.MoveTo P.PLoc
      match C.BGO P.Loc with
      | Color.Black -> P.MoveTo P.Loc
      | Color.Cyan ->
        P.MoveTo (rand C.Rows-1, rand C.Cols-1)
        while NoAreas P C || NoNoAreas P C do
          P.MoveTo (rand C.Rows-1, rand C.Cols-1)
      | _ ->
        match (List.tryFind (fun (i:Item) -> i.Loc = P.Loc) ItemList) with
        | Some i ->
          do i.InteractWith P |> ignore
          if false then
            ItemList <- 
              remove (List.findIndex (fun x -> x = i) ItemList) ItemList
          if i.FullyOccupy then
            P.MoveTo P.PLoc
          else
            P.MoveTo P.Loc
        | None -> P.MoveTo P.Loc
      if P.IsDead then printfn "You gone homie"
      if P.Win then printfn "You won homie"
  ///<returns >Enten vinder man ellers så taber man, repræsenteret ved
  /// de to print-statements</returns>