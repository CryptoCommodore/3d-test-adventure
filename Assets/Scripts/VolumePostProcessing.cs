using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
 

public class VolumePostProcessing : MonoBehaviour
{
    // Start is called before the first frame update
    public Volume volume;
    Bloom b;
    MotionBlur mb;
    ChromaticAberration ca;
    LensDistortion ld;
    LiftGammaGain lgg;
    Vignette v;
    PaniniProjection pp;
    ChannelMixer cm;
    ColorAdjustments coa;


    private GameObject[] gameObjects; 
    void Start()
    {
        volume.profile.TryGet<Bloom>(out b);
        volume.profile.TryGet<MotionBlur>(out mb);
        volume.profile.TryGet<ChromaticAberration>(out ca);
        volume.profile.TryGet<LensDistortion>(out ld);
        volume.profile.TryGet<LiftGammaGain>(out lgg);
        volume.profile.TryGet<Vignette>(out v);
        volume.profile.TryGet<PaniniProjection>(out pp);
        volume.profile.TryGet<ChannelMixer>(out cm);
        volume.profile.TryGet<ColorAdjustments>(out coa);
    }

    void Update()
    {
        if(Input.GetKeyDown("q")){
            StartCoroutine(MotionBlur());
        }
        if(Input.GetKeyDown("e")){
            StartCoroutine(MotionBlur());
        }
        if(Input.GetKeyDown("h")){
            StartCoroutine(BoxSpawn());
        }
        if(Input.GetKeyDown(KeyCode.Tab)){
            StartCoroutine(SlowMotion());
        }
        if(Input.GetKey(KeyCode.LeftControl)){
            StartCoroutine(Rewind());
        }
        else if(Input.GetKeyUp(KeyCode.LeftControl)){
            coa.postExposure.value = .1f;
            coa.contrast.value = 4;
            coa.colorFilter.value = new Color(1f, 1f , 1f );
            coa.hueShift.value = 0;
            coa.saturation.value = -10;
            ca.intensity.value = 0; 
            ld.intensity.value = 0 ;
        }

    }
    IEnumerator MotionBlur(){
        yield return new WaitForSeconds(.4f);
        mb.quality.Override(MotionBlurQuality.High);
        mb.intensity.value = 1;
        ca.intensity.value = 1;
        ld.intensity.value = .4f;
        yield return new WaitForSeconds(.6f);
        mb.intensity.value = 0.1f;
        ca.intensity.value = 0;
        ld.intensity.value = 0;
    }
    IEnumerator BoxSpawn(){
        float initialBloomValue =  b.intensity.value;
        float initialThreshold = b.threshold.value;
        yield return new WaitForSeconds(.4f);
        b.intensity.value = .2f;
        b.threshold.value = 1;
        yield return new WaitForSeconds(.5f);
        b.intensity.value = initialBloomValue;
        b.threshold.value = initialThreshold;
    }  
    IEnumerator SlowMotion(){
        v.color.value = new Color(.1527f , .5205f , .9811f);
        v.intensity.value = .788f;
        ca.intensity.value = .444f;
        pp.distance.value = .365f;

        cm.redOutRedIn.value = 160;
        cm.redOutGreenIn.value = 200;
        cm.greenOutRedIn.value = 40;
        cm.greenOutGreenIn.value = 100;
        yield return new WaitForSeconds(8f);
        StartCoroutine(Flicker());
    }
    IEnumerator Flicker(){

        v.color.value = new Color(.1527f , .5205f , .9811f);
        v.intensity.value = .788f;
        ca.intensity.value = .444f;
        pp.distance.value = .365f;

        cm.redOutRedIn.value = 160;
        cm.redOutGreenIn.value = 200;
        cm.greenOutRedIn.value = 40;
        cm.greenOutGreenIn.value = 100;
        yield return new WaitForSeconds(.1f);
        v.color.value = new Color(0f , 0f , 0f);
        v.intensity.value = 0f;
        ca.intensity.value = 0f;
        pp.distance.value = 0f;

        cm.redOutRedIn.value = 100;
        cm.redOutGreenIn.value = 0;
        cm.greenOutRedIn.value = 0;
        cm.greenOutGreenIn.value = 100;
    }
    IEnumerator Rewind(){
    coa.postExposure.value = .15f;
    coa.colorFilter.value = new Color(.4136f, 1.7956f , 1.6111f );
    coa.hueShift.value = 46;
    coa.saturation.value = -10;
    ca.intensity.value = Mathf.PingPong(Time.time*3,1);
    ld.intensity.value = Mathf.PingPong(Time.time*3 , .3f); 
    yield return true;

    }
}
