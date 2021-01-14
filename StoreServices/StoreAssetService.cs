using Microsoft.EntityFrameworkCore;
using StoreData;
using StoreData.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreServices
{
    public class StoreAssetService : IStoreAsset

    {
        private StoreContext _context;

        public StoreAssetService(StoreContext context)
        {
            _context = context;
        }


        public void Add(StoreAsset newAsset)
        {
            _context.Add(newAsset);
            _context.SaveChanges();
        }

        public IEnumerable<StoreAsset> GetAll()
        {
            return _context.StoreAssets
                .Include(asset => asset.Status)
                .Include(asset => asset.Location);
        }


        public StoreAsset GetById(int id)
        {
            return
                 GetAll()
                 .FirstOrDefault(asset => asset.Id == id);
        }

        public StoreBranch GetCurrentLocation(int id)
        {
            return GetById(id).Location;
            //return _context.StoreAssets.FirstOrDefault(asset => asset.Id == id).Location;
        }

        public string GetDeweyIndex(int id)
        {
            if (_context.Music.Any(music => music.Id == id))
            {
                return _context.Music.FirstOrDefault(music => music.Id == id).DeweyIndex;
            }

            else return "";
        }

        public string GetIsbn(int id)
        {
            if (_context.Music.Any(a => a.Id == id))
            {
                return _context.Music
                    .FirstOrDefault(a => a.Id == id).ISBN;
            }

            else return "";

        }

        public string GetTitle(int id)
        {
            return _context.StoreAssets
                .FirstOrDefault(a => a.Id == id)
                .Title;
        }

        public string GetType(int id)
        {
            var music = _context.StoreAssets.OfType<Music>()
                .Where(m => m.Id == id);

            return music.Any() ? "Music" : "Video";
        }
        public string GetAuthorOrDirector(int id)
        {
            var isMusic = _context.StoreAssets.OfType<Music>()
                .Where(asset => asset.Id == id).Any();

            var isVide = _context.StoreAssets.OfType<Video>()
                .Where(asset => asset.Id == id).Any();

            return isMusic ?
                _context.Music.FirstOrDefault(music => music.Id == id).Author :
                _context.Videos.FirstOrDefault(video => video.Id == id).Director
                ?? "Unknown";
        }
    }
}
