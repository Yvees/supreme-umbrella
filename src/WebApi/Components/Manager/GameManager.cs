using MagCore.Sdk.Helper;
using MagCore.Sdk.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemCommonLibrary.Json;
using WebApi.Components.Network;
using WebApi.Exceptions;
using WebApi.Models;

namespace WebApi.Components.Manager
{
    public static class GameManager
    {
        public static async Task<(string gid, string pid, int pidx)> JoinOneGame(bool isMatch, string map, string name, int color, string openid)
        {
            var host = string.Empty;
            if (isMatch)
                host = HelperConfig.Current.MagcoreApiServer;
            else
                host = HelperConfig.Current.MagcoreApiTrain;

            ServerHelper.Initialize(host);

            var gameList = GameHelper.GameList();

            if (gameList == null || gameList.Length == 0)
            {
                //no games exist
                return await JoinNewGame(isMatch, map, name, color, openid);
            }
            else
            {
                var gameMap = MapHelper.GetMap(map);
                for (int i = 0; i < gameList.Length; i++)
                {
                    if (gameList[i].state == 0 && gameList[i].map == map)
                    {
                        Game game = new Game(gameMap.Rows.Count, gameMap.Rows[0].Count);
                        if (GameHelper.GetGame(gameList[i].id.ToString(), ref game) && game != null)
                        {
                            if (!game.Players.Exists(p => p.Color == color))
                            {
                                //found a game match
                                var player = PlayerHelper.CreatePlayer(name, color);
                                if (player == null)
                                    throw new GameJoinedException();
        
                                GameHelper.JoinGame(game.Id, player.Id);
                                var score = new GameScore()
                                {
                                    oid = openid,
                                    pid = player.Id,
                                    gid = game.Id,
                                    pidx = player.Index,
                                    ismatch = isMatch,
                                    score = 0
                                };

                                await DbEntityManager.Insert(score);

                                return (gid: game.Id, pid: player.Id, pidx: player.Index);
                            }
                        }
                    }
                }

                //found no game match
                return await JoinNewGame(isMatch, map, name, color, openid);
            }
        }

        private static async Task<(string gid, string pid, int pidx)> JoinNewGame(bool isMatch, string map, string name, int color, string openid)
        {
            var player = PlayerHelper.CreatePlayer(name, color);
            if (player == null)
                throw new GameJoinedException();

            var args = new Dictionary<string, object>();
            args.Add("Feedback", HelperConfig.Current.WxInterfaceHost + "api/GameHelper/Finish");
            var gameid = GameHelper.CreateGame(map, args);
            GameHelper.JoinGame(gameid, player.Id);

            var score = new GameScore() {
                oid = openid,
                pid = player.Id,
                gid = gameid,
                pidx = player.Index,
                ismatch = isMatch,
                score = 0
            };

            await DbEntityManager.Insert(score);

            return (gid: gameid, pid: player.Id, pidx: player.Index);
        }

    }
}
