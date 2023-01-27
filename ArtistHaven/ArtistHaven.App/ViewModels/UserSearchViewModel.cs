using ArtistHaven.App.Data;
using ArtistHaven.Shared.DTOs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ArtistHaven.App.ViewModels {
    public class UserSearchViewModel : INotifyPropertyChanged {
        public event PropertyChangedEventHandler PropertyChanged;
        private UserManager _userManager;
        public UserSearchViewModel(UserManager userManager) {
            _userManager = userManager;
        }

        protected virtual void NotifyPropertyChanged([CallerMemberName] string propertyName = "") {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public ICommand PerformSearch => new Command<string>(async (string query) =>
        {
            SearchResults = await _userManager.FindUsers(query);
        });

        private List<PublicUserDTO> searchResults = new();
        public List<PublicUserDTO> SearchResults {
            get {
                return searchResults;
            }
            set {
                searchResults = value;
                NotifyPropertyChanged();
            }
        }
    }
}
