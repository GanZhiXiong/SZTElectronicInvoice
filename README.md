# SZTElectronicInvoice
深圳通充值发票手动、自动批量下载程序 

**若快倒了🤷🤷，试过了其他付费验证码自动识别平台，识别率都很低，都比不过若快。虽然自己之前写过验证码识别，但是只能识别字母和数字，对于这种三位数相加的验证码识别还是有点难度的，我这里自己去实现或找第三方开源识别库了，也不会再去找付费验证码识别平台了。️**  

**🤦🤦现在程序去掉了若快验证码识别，改为弹框手动输入验证码**  

![](https://github.com/GanZhiXiong/SZTElectronicInvoice/blob/master/Images/auto_download.gif)

## 导航
- [为什么要写这个程序](#为什么要写这个程序)
- [简介](#简介)  
- [详细介绍](#详细介绍)
  - [自动](#自动)   
  - [手动](#手动)
  - [配置 ](#配置)
- [问题](#问题)
  - [下载后的发票文件在哪里](#下载后的发票文件在哪里)   
  - [不能自动下载发票](#不能自动下载发票)
  - [下载后的pdf发票文件，一张张打印太麻烦了](#下载后的pdf发票文件，一张张打印太麻烦了)
  - [pdf打印能否一张纸打印两个发票](#pdf打印能否一张纸打印两个发票)
 
- [有问题反馈](#有问题反馈)
- [感激](#感激)
- [捐助开发者](#捐助开发者)
- [关于作者](#关于作者)

## 为什么要写这个程序
我们来看下网页中下载发票的步骤：  
**1、进入深圳通电子发票页面**  
![](https://github.com/GanZhiXiong/SZTElectronicInvoice/blob/master/Images/1.png)

**2、输入查询信息**  
![](https://github.com/GanZhiXiong/SZTElectronicInvoice/blob/master/Images/2.png)  
卡号：9位数字的卡号  
交易日期：只点击文本框右边的日历小图标，然后从中选择一个日期，这个日期控件用起来可真麻烦，只能显示一个月，你就不能多显示3个月吗  
验证码：3位数字相加，算起来简单，可还是会有人算错的，一算错就烦，一烦躁就不想下载发票了 
  
输入完成点击查询

**3、申请电子发票**  
![](https://github.com/GanZhiXiong/SZTElectronicInvoice/blob/master/Images/3.png)  
勾选要申请的电子发票【这个勾选就是多余啊，这里只会显示一张发票，不选这张还能选其他的吗  
点击申请电子发票  

**4、填写发票对象信息**  
![](https://github.com/GanZhiXiong/SZTElectronicInvoice/blob/master/Images/填写发票信息.png)  
**个人**  
这个要填的信息就两个，还好  
**单位**   
这个就比较多了，必填是3个，选填是4个，一共7个，当然我们一般情况下就只需填必填的3个
这个三个必填的都比较长，我相信你一定是复制粘贴填进去，可能还有就是设置了快捷键文本替换【Mac自带这个功能、Windows可以送AutoHotKey这类工具】  

输入完成后点击提交


**5、下载发票**  
![](https://github.com/GanZhiXiong/SZTElectronicInvoice/blob/master/Images/5.png)  
服务器先生存pdf发票文件，然后在生成一个链接给你下载，所以下载快慢不能完全取决于你的网速，服务器生成文件慢，那可能就是上图的一直转圈了

> **下载发票要经过上面这么麻烦的5个过程，真是浪费时间啊，作为一个程序员，是不会把时间浪费在这重复繁琐的操作上的，因为我写了这样一个程序**  

## 简介 
程序支持手动下载发票、自动下载发票、设置公司抬头配置信息等……  
**操作中验证码自动识别、小票照片自动识别**  
具体请往下看  

## 详细介绍
### 自动
![](https://github.com/GanZhiXiong/SZTElectronicInvoice/blob/master/Images/自动1.png)
![](https://github.com/GanZhiXiong/SZTElectronicInvoice/blob/master/Images/自动2.png)  
1、请用手机拍摄深圳通电子发票  
2、通过手机微信将照片发送到电脑微信【更好的方法是通过手机QQ的“传文件/照片到电脑”这个功能将拍摄好的发票照片发送到电脑，因为电脑QQ会自动保存到“D:\Program Files\QQRecord\QQ号码\FileRecv\MobileFile”这个目录下面，这多方便，在程序中直接选这个目录就可以了】  
3、将照片保存到一个文件夹中  
4、选择电子发票照片所在的文件夹，点击“开始批量下载发票”按钮即可自动  
   识别照片上发票信息，自动下载发票

**看第二张图，虽然程序自动识别文字还不是那么完美，但是我代码已经做了处理，所以即使部分模糊不清不能识别，程序也可能会下载到发票的**  

**自动下载目前只支持【清湖地铁站的小票】，如需支持其他地铁站的小票，请自行Fork，欢迎大家踊跃加入本项目**

### 手动
![](https://github.com/GanZhiXiong/SZTElectronicInvoice/blob/master/Images/手动1.png)  
手动下载发票只需要填写卡号和交易日期，验证码自动识别，操作起来是不是很简单
**有了自动为什么还要做一个手动呢？就是因为自动下载目前只支持【清湖地铁站的小票】**

### 配置  
![](https://github.com/GanZhiXiong/SZTElectronicInvoice/blob/master/Images/配置.png)  
你可以这里配置很多公司和纳税人识别号  
而且还可以设置是否跳过已下载的发票文件  
【
如果该发票文件之前已经生成过（可能下载了，也可能只是生成了而未下载），则跳过不下载。  
如下情况可以勾选该选项：  
1、防止重复下载该发票文件；  
2、防止下载的发票公司信息不是配置的公司（发票已被其他人生成过）。】
   
## 问题        
### 下载后的发票文件在哪里
发票文件放在程序所在目录的“ZhiXiongDownload”目录下面  
且程序会自动将按照公司名称将发票金额归类，放入到对应金额的目录下面

### 不能自动下载发票
自动识别验证码是调用付费的第三方接口，是著者自己购买充值的，费用是按照识别验证码个数算的，用完了就没有了。  
著者开发这个软件也不容易，而且免费开源，需要付费的验证码那就需要大家的支持了，你可以给著者捐助个几元钱，当然越多越好。捐助后著者会将钱充值到自动识别验证码第三方接口平台中。   
**如需捐助的，请往下滑，看到二维码即可支付宝或微信扫描完成捐助**

### 下载后的pdf发票文件，一张张打印太麻烦了
可以使用仓库中的“PDFBinder_V1.2_XiTongZhiJia.rar”中工具，能将多个pdf合并成为一个pdf然后打印那一个pdf文件即可

### pdf打印能否一张纸打印两个发票
可以使用福昕pdf软件来打印，在打印的时候选择每张纸上放置多页，选择每页版数为2即可。  
福昕pdf软件自行百度下载，不打广告

## 有问题反馈
在使用中有任何问题，欢迎反馈给我。

## 捐助开发者
有钱捧个钱场，没钱捧个人场（请点击页面右上角的★Star，给个Star呗），谢谢各位。

![](https://github.com/GanZhiXiong/ZXLPR/blob/master/Images/alipay_qrcode.png)
![](https://github.com/GanZhiXiong/ZXLPR/blob/master/Images/weixinpay_qrcode.png)

<!--
<div style="text-align:center;">
    <div style="display:inline-block<p></p>;"><img src="https://github.com/GanZhiXiong/ZhiXiongYouDaoNoteInstallationPackage/blob/master/images/Pay/AlipayQRCode.jpg"></div>
    <div style="display:inline-block;margin-left:40px;"><img src="https://github.com/GanZhiXiong/ZhiXiongYouDaoNoteInstallationPackage/blob/master/images/Pay/weixinpay_qrcode.jpg"></div>
    <div style="font-weight:bold;margin-top:15px;">您的支持是我持续开发的最大动力。
        <br>退款没有有效期，只需要提供付款截图和收款二维码即可（不是二维码名片）
        <br>如需退款请发邮件至：ganzhixiong@sina.cn
    </div>
</div>
-->

## 感激
  

## 关于作者

```javascript
  var coder = {
	"nickName": "干志雄",
    "email": "ganzhixiong@sina.cn",
    "qq": "1551935335",
    "site": [
        "http://www.xinweijs.com",
        "http://www.ganzhixiong.com"
    ]
}
```
