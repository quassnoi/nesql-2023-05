SELECT  TOP 2 *
FROM    OPENJSON(
        N'[{"id":1,"value":"some text"},
        {"id":2,"value":"another text"}, T̶̟̘̯̬h̢̭͍̯̝͚̣̼e̮͟ ͈̰̳̣d̦͔͞a̛̗͈͎̭w̸̘̭̳̳̜n͖̼ ̥͉o̼f̫̝̟̣̭͈͞ ̫̻͈̻͈t͔̲̠͚h̭̙ḙ ̶̟̲͖ę̫̯̬n̴̼̯d̖̥̰͚̖̰͙ ̵̞̘̮i͓͓̤̼s̷̝͚̬ ̯̥͖c̯̫o̯̼̤m̭̺͈i̭ṉ̦̮̙g̳̳
        H̻̲e͓̞̤͕ ̸̲̭̖͉͔͔͖w͚͕̝̺̖͈͠h̘̣͖̮̲̣̟͘o̶̫̜̜͍ ̴̜͚͈͖s͕̗̘͈̪̠̻̯l̶̤̰͕̭̝̲̰i̖̼̪̠͈͈̩̻t̖̳͠h̬͉e̺̺͈̙r̲̭͔̟͎s͈͈̳̣͞Y̲̪̖o̲̘̫u̵̺͇̮ ͈͈͓̩̙̖c͘a̭̻͚̫n̡̼̩̫̜̰̪t̸̯ ̯̫̰̯r̻̲u̝n̜̥̜̜ ̠̞̕f this is not JSON ạ̰̦̳̎̀ͬ̃ͥ̔̓ͪ̈̔ͪ͒͘͢l̪̥̯͔̳̺̱̰̾̾͗ͥ̌͐̀͢l̸̛͍̗͔̦͉̅ͣ́̓ͩ̇̽̓͑ͥ̈́ͯ͘͜͢ͅ ̵͔͕̖͔͈͎͍̪̱̮̼̤ͣ̊̇̎̎́͢͠l̛̎ͤͭ̿̿̏͒̈́̈́̀͏̨̘̗̱̩̖͖̬̕ŏ̵̵̵̢̳̤̣̱̖͙̱̲̻̇̄ͨͬ͗ͯ̓̀̚ͅs̵̸̮͕͉͙̭̤̒͋̀̊ͧͭ̅͑̊ͦ̂ͥ͘͝ţ̷̙͓̦͔̗̥̹͙̟̱̣͖͓͔̩̿̾̉ͧ͛̓͌ͫ̆̓ͥ͛ͧ̽͠
        ')
WITH    (id BIGINT, value NVARCHAR(MAX))