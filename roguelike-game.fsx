open Game
open System

let StandardMap = Canvas(17,32)
let lvl1 = World(StandardMap)
lvl1.AddItem (Exit(16,30))
for i=0 to 16 do
  lvl1.AddItem (Wall2(i,0))
  lvl1.AddItem (Wall2(i,31))
for i=2 to 29 do
  lvl1.AddItem (Wall2(0,i))
  lvl1.AddItem (Wall2(16,i))
lvl1.AddItem (Wall2(16,1))
lvl1.AddItem (Wall2(0,30))
for i=6 to 15 do
    for j=25 to 26 do
      lvl1.AddItem (Wall(i,j))
for i = 1 to 9 do
  for j = 17 to 18 do
    lvl1.AddItem (Wall(i,j))
for i = 8 to 14 do
  for j= 20 to 21 do
   lvl1.AddItem (Wall(i,j))
for i=11 to 15 do
  for j=16 to 17 do
    lvl1.AddItem (Wall(i,j))
for i=1 to 13 do
  for j=3 to 4 do
    lvl1.AddItem (Wall(i,j))
for i=8 to 9 do
  for j=5 to 16 do
    lvl1.AddItem (Wall(i,j))
for i=1 to 7 do
  for j=5 to 16 do
    lvl1.AddItem (Fire(i,j))
for i=14 to 15 do
 lvl1.AddItem (Wall(i,3))
lvl1.AddItem (Wall(14,5))
lvl1.AddItem (Wall(13,5))
lvl1.AddItem (Wall(14,6))
lvl1.AddItem (Wall(13,6))
lvl1.AddItem (Wall(15,5))
lvl1.AddItem (Wall(15,8))
lvl1.AddItem (Wall(14,7))
lvl1.AddItem (Wall(11,5))
lvl1.AddItem (Wall(10,6))
lvl1.AddItem (Wall(11,7))
lvl1.AddItem (Wall(12,8))
lvl1.AddItem (FleshEatingPlant(13,8))
lvl1.AddItem (Wall(15,10))
lvl1.AddItem (Wall(15,11))
lvl1.AddItem (Wall(14,9))
lvl1.AddItem (Wall(12,9))
lvl1.AddItem (Wall(11,9))
lvl1.AddItem (Wall(11,8))
lvl1.AddItem (Wall(10,15))
lvl1.AddItem (Wall(10,13))
lvl1.AddItem (Wall(11,14))
lvl1.AddItem (Wall(12,14))
lvl1.AddItem (Wall(11,12))
lvl1.AddItem (Wall(12,12))
for i=12 to 15 do
  lvl1.AddItem (Wall(13,i))
lvl1.AddItem (Wall(11,10))
lvl1.AddItem (Wall(11,11))
lvl1.AddItem (Wall(12,10))
lvl1.AddItem (Wall(12,11))
lvl1.AddItem (Wall(14,11))
lvl1.AddItem (Wall(14,13))
lvl1.AddItem (Wall(15,15))
lvl1.AddItem (Wall(15,12))
lvl1.AddItem (Wall(8,19))
lvl1.AddItem (Wall(9,19))
lvl1.AddItem (Wall(14,29))
lvl1.AddItem (Wall(14,30))
lvl1.AddItem (Wall(13,29))
lvl1.AddItem (Wall(13,30))
lvl1.AddItem (Wall(13,28))
lvl1.AddItem (Wall(14,28))
lvl1.AddItem (Wall (11,27))
lvl1.AddItem (Wall (11,28))
lvl1.AddItem (Wall (10,27))
lvl1.AddItem (Wall (10,28))
lvl1.AddItem (Wall (11,29))
lvl1.AddItem (Wall (10,29))
lvl1.AddItem (Wall (11,30))
lvl1.AddItem (Wall (9,30))
lvl1.AddItem (Wall (8,29))
lvl1.AddItem (Wall (8,28))
lvl1.AddItem (Wall (8,27))
lvl1.AddItem (Wall (7,29))
lvl1.AddItem (Wall (6,30))
lvl1.AddItem (Wall (6,29))
lvl1.AddItem (Wall (6,28))
lvl1.AddItem (Wall (5,29))
lvl1.AddItem (Wall (5,27))
lvl1.AddItem (Wall (4,30))
lvl1.AddItem (Wall (4,29))
lvl1.AddItem (Wall (4,28))
lvl1.AddItem (Wall (7,28))
lvl1.AddItem (Water(9,29))
lvl1.AddItem (Water(9,28))
for i=1 to 2 do
  for j=19 to 21 do
    lvl1.AddItem (Water(i,j))
for i=6 to 7 do
  for j=19 to 24 do
    lvl1.AddItem (Water(i,j))
lvl1.AddItem (Water(15,20))
lvl1.AddItem (Fire(12,28))
for i=1 to 2 do
  for j=22 to 30 do
    lvl1.AddItem (Fire(i,j))
for i in 3 .. 2 .. 5 do
  for j=19 to 21 do
    lvl1.AddItem (Fire(i,j))
for i in 8..2..14 do
  for j=22 to 24 do
    lvl1.AddItem (Fire(i,j))
lvl1.AddItem (FleshEatingPlant(15,21))
lvl1.AddItem (FleshEatingPlant(4,21))
lvl1.AddItem (FleshEatingPlant(4,20))
lvl1.AddItem (FleshEatingPlant(13,27))
lvl1.AddItem (FleshEatingPlant(15,28))
lvl1.AddItem (FleshEatingPlant(13,22))
lvl1.AddItem (FleshEatingPlant(13,24))
lvl1.AddItem (FleshEatingPlant(11,22))
lvl1.AddItem (FleshEatingPlant(11,23))
lvl1.AddItem (FleshEatingPlant(9,23))
lvl1.AddItem (FleshEatingPlant(9,24))
for i=22 to 30 do
  lvl1.AddItem (FleshEatingPlant(3,i))
for i=22 to 25 do
  for j=4 to 5 do
    lvl1.AddItem (FleshEatingPlant (j,i))
lvl1.AddItem (Teleport(1,2))
lvl1.AddItem (Teleport(15,1))
lvl1.AddItem (Teleport(15,2))
let game =
  lvl1.Play()
game
