using MM2Randomizer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MM2RandoHost.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private RandoSettings _randoSettings;
        private bool _isCoreModulesChecked = true;
        private bool _isShowingHint = true;

        public MainWindowViewModel()
        {
            RandoSettings = new RandoSettings();
            RandomMM2.Settings = RandoSettings;

            // Try to load "MM2.nes" if one is in the local directory already to save time
            if (System.IO.File.Exists("MM2.nes"))
            {
                RandoSettings.ValidateFile("MM2.nes");
                IsShowingHint = false;
            }
        }

        public RandoSettings RandoSettings
        {
            get => _randoSettings;
            set
            {
                if (_randoSettings != value)
                {
                    _randoSettings = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsCoreModulesChecked
        {
            get => _isCoreModulesChecked;
            set
            {
                if (_isCoreModulesChecked != value)
                {
                    _isCoreModulesChecked = value;
                    RandoSettings.Is8StagesRandom = value;
                    RandoSettings.IsWeaponsRandom = value;
                    RandoSettings.IsTeleportersRandom = value;
                    NotifyPropertyChanged();
                }
            }
        }

        public bool IsShowingHint
        {
            get => _isShowingHint;
            set
            {
                if (_isShowingHint != value)
                {
                    _isShowingHint = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
