# Makefile for building POSPrinterApp and Electron app

# Variables
POS_PRINTER_APP_DIR = ./PosPrinterApp
POS_APP_DIR = ./WebApplication5
EXE_PATH = $(POS_PRINTER_APP_DIR)/publish/PosPrinterApp.exe
RESOURCES_DIR = $(POS_APP_DIR)/resources

# Targets

# Target to build POSPrinterApp (console .exe)
build_pos_printer:
	cd $(POS_PRINTER_APP_DIR) && dotnet publish -c Release -r win-x64 --self-contained --output ./publish

# Target to copy .exe to Electron app's resources folder
copy_exe_to_resources: build_pos_printer
	mkdir -p $(RESOURCES_DIR)
	cp $(EXE_PATH) $(POS_APP_DIR)/PosPrinterApp.exe

# Target to build Electron app with electronize
build_electron:
	cd $(POS_APP_DIR) && electronize build /target win --win --dir

# Full build process
build: copy_exe_to_resources build_electron

run:
	cd $(POS_APP_DIR) && dotnet electronize start

# Clean up build artifacts
clean:
	rm -rf $(POS_PRINTER_APP_DIR)/bin
	rm -rf $(POS_APP_DIR)/dist
	rm -rf $(POS_APP_DIR)/node_modules
