using MM2Randomizer;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Reflection;

namespace MM2RandoHost.ViewModels
{
    public class MainWindowViewModel : ViewModelBase
    {
        private RandoSettings _randoSettings;
        private bool _isCoreModulesChecked = true;
        private bool _isShowingHint = true;
        private bool _hasGeneratedAROM = false;

        public MainWindowViewModel()
        {
            RandoSettings = new RandoSettings();
            RandomMM2.Settings = RandoSettings;

            // Try to load "MM2.nes" if one is in the local directory already to save time
            string tryLocalpath = Path.Combine(
                Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                "MM2.nes");

            if (File.Exists(tryLocalpath))
            {
                RandoSettings.ValidateFile(tryLocalpath);
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

        public bool HasGeneratedAROM
        {
            get => _hasGeneratedAROM;
            set
            {
                if (_hasGeneratedAROM != value)
                {
                    _hasGeneratedAROM = value;
                    NotifyPropertyChanged();
                }
            }
        }
    }
}
