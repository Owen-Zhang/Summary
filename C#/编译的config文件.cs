在build Events 中加上  echo f|xcopy /Y "$(ProjectDir)App.$(ConfigurationName).config" "$(TargetPath).config"
项目中有: App.GQC.config, App.Release.config等
