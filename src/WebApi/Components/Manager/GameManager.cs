using MagCore.Sdk.Helper;
using MagCore.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemCommonLibrary.Json;
using WebApi.Components.Network;
using WebApi.Models;

namespace WebApi.Components.Manager
{
    public static class GameManager
    {
        public static async Task<string> JoinOneGame(bool isTrain, string map, string name, int color, string openid)
        {
            var host = string.Empty;
            if (isTrain)
                host = HelperConfig.Current.MagcoreApiTrain;
            else
                host = HelperConfig.Current.MagcoreApiTrain;

            ServerHelper.Initialize(host);

            var gameList = GameHelper.GameList();

            if (gameList == null || gameList.Length == 0)
            {
                //no games exist
                return await JoinNewGame(map, name, color, openid);
            }
            else
            {
                for (int i = 0; i < gameList.Length; i++)
                {
                    if (gameList[i].state == 0 && gameList[i].map == map)
                    {
                        Game game = null;
                        if (GameHelper.GetGame(gameList[i].id.ToString(), ref game) && game != null)
                        {
                            if (!game.Players.Exists(p => p.Color == color))
                            {
                                //found a game match
                                var player = PlayerHelper.CreatePlayer(name, color);
                                GameHelper.JoinGame(game.Id, player.Id);
                                var score = new GameScore()
                                {
                                    oid = openid,
                                    pid = player.Id,
                                    gid = game.Id
                                };

                                await DbEntityManager.Insert(score);

                                return game.Id;
                            }
                        }
                    }
                }

                //found no game match
                return await JoinNewGame(map, name, color, openid);
            }
        }

        private static async Task<string> JoinNewGame(string map, string name, int color, string openid)
        {
            var player = PlayerHelper.CreatePlayer(name, color);
            var gameid = GameHelper.CreateGame(map);
            GameHelper.JoinGame(gameid, player.Id);

            var score = new GameScore() {
                oid = openid,
                pid = player.Id,
                gid = gameid
            };

            await DbEntityManager.Insert(score);

            return gameid;
        }

    }
}
