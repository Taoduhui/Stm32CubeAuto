# Stm32CubeAuto
Stm32Cubeide Auto Completion Tool

Release https://github.com/Taoduhui/Stm32CubeAuto/releases/latest

## 自动提示逻辑

1. 'A'-'Z' 以及 短横线会触发自动补全，在停止输入500ms后自动补全
2. 在经由步骤1触发自动补全并停止输入后，当键盘上有按键仍有按键按住时不会触发自动补全，在松开后500ms后自动补全
3. 当LeftCtrl按下/住时，会取消本次的自动补全任务
