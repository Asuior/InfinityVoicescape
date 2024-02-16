

<div align="center">


# SO-VITS-SVC&Bert-VITS2整合包


</div>



**本项目是[SO-VITS-SVC](https://github.com/svc-develop-team/so-vits-svc/)和[Bert-VITS2](https://github.com/fishaudio/Bert-VITS2)项目的整合，使用时请遵守二者的使用规范**



## 下载链接
- [Google Drive](https://drive.google.com/file/d/1UvtGboRwYyKeem_jVcV-6L4FTc26dfUe/view?usp=drive_link)
- [Baidu (百度云盘) ](https://pan.baidu.com/s/1OS-EYueiL5c_2oPfmvwZLw?pwd=x5yl)



## 后台服务
两种启动方式选择其一

### 1. 在程序中启动
在程序的服务页面填写ip和端口号，点击启动服务<br/>
<br/>
- IP地址若不需要从外部访问填写127.0.0.1即可，若需要则填写0.0.0.0<br/> 
当设置为0.0.0.0时，在另一台设备上启动程序，将服务页面的ip和端口修改为启动服务的设备的ip和服务所设置的端口(此时无需点击启动服务)也可以正常使用<br/><br/>
- 端口号填写一个未被使用的端口


### 2. 手动启动
将根目录下server.py以文本文件方式打开，将文件最下方的
```python
parser.add_argument("-l", "--listen", type=int, required=True, help="父进程")
args = parser.parse_args()
threading.Thread(target=prot).start()
```
修改为
```python
#parser.add_argument("-l", "--listen", type=int, required=True, help="父进程")
args = parser.parse_args()
#threading.Thread(target=prot).start()
```
即在这三行中的第一行与第三行前加上#号（若要更换为使用第一种方式启动，请将此时添加的#号删除）
随后在根目录打开终端，执行
```shell
.\Vs\python.exe .\server.py 
```
参数
- -H ip地址,默认为127.0.0.1
- -p 端口号,默认为1999

启动程序，将服务界面的IP地址和端口号修改为与启动的服务一致(此时无需点击启动服务)


## 模型存放位置
在程序的根目录下有BertVITS2和sovitssvc两个文件夹，分别对应Bert-VITS2与SO-VITS-SVC，两个文件夹中各有一个Model文件夹，将对应的模型和配置文件放入其中。


## 推理

SVC/BV2为推理页面，在服务启动后，点击页面上方的刷新按钮获取模型和配置文件，选择想使用的模型，配置文件和设备后点击加载模型，之后可按需要调整参数，最后点击生成，生成的音频和音频位置会在页面下方显示。





## 训练

### 数据集准备

- Bert-VITS2数据集<br/>
  结构应为
  ```
  数据集存放目录名
    ├───说话人名称
    │   └───语言
    │       ├───音频文件名1.wav
    │       └───音频文件名1.txt
    │   
    │
    └───说话人名称
        ├───语言
        │   ├───音频文件名2.wav
        │   └───音频文件名2.txt
        └───语言
            ├───音频文件名3.wav
            └───音频文件名3.txt
  ```
  音频文件名可任意填写，但语音对应的台词需保存在与语音同名的txt文件中(若在生成时选择自动生成文本，则中文和英文的语音可缺少台词文件)，语言可选项为ZH，JP，EN，分别对应中文日文英文<br/>
  例如我有一个数据集放在A目录，包含M和N两个角色的语音，角色M的语音有包含中文和日语两种语言，角色N的语音只有英语，则文件夹结构应为
    ```
    A
    ├───M
    │   ├───ZH
    │   │   ├───音频文件名1.wav
    │   │   ├───音频文件名1.txt
    │   │   └───...
    │   └───JP
    │       ├───音频文件名1.wav
    │       ├───音频文件名1.txt
    │       └───...
    │   
    └───N
        └───EN
            ├───音频文件名1.wav
            ├───音频文件名1.txt
            └───...

    ```


- SO-VITS-SVC数据集<br/>
  与Bert-VITS2数据集只是减少了语言这一层目录并且只需提供音频文件<br/>
  结构应为
  ```
  数据集存放目录名
    ├───说话人名称
    │   └───音频文件名1.wav
    │   
    │
    └───说话人名称
        └───音频文件名1.wav
  ```


### 开始训练
在程序的Train页面顶部的生成选择想要生成的模型，点击选择目录选取数据集存放目录，根据需要调整参数后点击开始训练<br/>
- Bert-VITS2模型保存位置：BertVITS2\data<br/>
- SO-VITS-SVC模型保存位置：sovitssvc\logs
