# PostProcessingExpansionPack
The repository is the expansion package for Unity's Post-processing Stack V2.
And there is the template utility to help developers create new custom post-processing easily.

## How to Create Template
* Right click in the project window.
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-grayscale-how-to-create-template-step-1.png">
</p>
* Naming the shader you want to create.
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-grayscale-how-to-create-template-step-2.png">
</p>
* There are two files would be created, the shader and the C# scripts.
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-grayscale-how-to-create-template-step-3.png">
</p>
* Writing the shader, custom the post-processing effect which you want.
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-grayscale-how-to-create-template-step-4.png">
</p>
* Writing the C# scripts, add the parameter and assign the value to the shader, and custom the rendering workflow in  PostProcessEffectRenderer.Render.
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-grayscale-how-to-create-template-step-5.png">
</p>
* Add the effect in PostProcessingVolume
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-grayscale-how-to-create-template-step-6.png">
</p>
* Result
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-grayscale-how-to-create-template-step-7.png">
</p>

## Custom Effects
Original
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-original.png">
</p>

AccumulationBuffer
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-accumulation-buffer.png">
</p>

Blur/BoxBlur
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-blur-box.png">
</p>

Blur/GussianBlur
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-blur-gaussian.png">
</p>

Blur/KawaseBlur
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-blur-kawase.png">
</p>

Checkerboard
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-checkerboard.png">
</p>

Chromatic Aberration
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-chromatic-aberration.png">
</p>

CRT
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-crt.png">
</p>

Cubic Lens Distortion
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-cubic-lens-distortion.png">
</p>

DOS
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-dos.png">
</p>

GameBoy
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-gameboy.png">
</p>

Grayscale
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-grayscale.png">
</p>

Global Fog
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-global-fog.png">
</p>

HSBC
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-hsbc.png">
</p>

Invert
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-invert.png">
</p>

Kernel/Edge Enhance 1
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-kernel-edge-enhance-1.png">
</p>

Kernel/Edge Enhance 2
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-kernel-edge-enhance-2.png">
</p>

Kernel/Edge Enhance 3
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-kernel-edge-enhance-3.png">
</p>

Kernel/Edge Enhance 4
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-kernel-edge-enhance-4.png">
</p>

Kernel/Emboss 3x3
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-kernel-emboss-3x3.png">
</p>

Kernel/Gradient-Prewitt 3x3
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-kernel-gradient-prewitt-3x3.png">
</p>

Kernel/Gradient Sobel 3x3
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-kernel-gradient-sobel-3x3.png">
</p>

Kernel/Motion Blur
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-kernel-motion-blur.png">
</p>

Kernel/Sharpen
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-kernel-sharpen.png">
</p>

Moasic
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-mosaic.png">
</p>

Noise
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-noise.png">
</p>

Oil Painting
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-oil-panting.png">
</p>

Outline
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-outline-normal.png">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-outline-depth.png">
</p>

Pencil
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-pencil.png">
</p>

Sepia
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-sepia.png">
</p>

Spotlight
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-spotlight.png">
</p>

ZBuffer
<p align="center">
<img style="margin:auto;"  src="https://github.com/ted10401/PostProcessingExpansionPack/blob/master/GithubResources/post-processing-expansion-pack-zbuffer.png">
</p>
