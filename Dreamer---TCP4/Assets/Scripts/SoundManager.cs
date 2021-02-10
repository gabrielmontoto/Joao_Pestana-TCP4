using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using System;
public class SoundManager : MonoBehaviour
{
    //  [SerializeField] Slider Efeito, Musica;
    [SerializeField] float VolumeEfeito, VolumeMusica;

    [SerializeField] Sound[] soundsEfeitos, soundsMusicas;

    [SerializeField] string musicaAtual;

    public static SoundManager Instance { get; set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        foreach (Sound s in soundsEfeitos)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

        foreach (Sound s in soundsMusicas)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;

            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
        }

    }
    private void Start()
    {

        VolumeEfeito = PlayerPrefs.GetFloat("VolumeEfeito");
        VolumeMusica = PlayerPrefs.GetFloat("VolumeMusica");
        if (PlayerPrefs.GetInt("Primeiro") == 0)
        {
            VolumeEfeito = 70;
            VolumeMusica = 70;
            PlayerPrefs.SetInt("Primeiro", 1);
        }
    }

    public void PlayEfeito(string nome)
    {
        Sound s = Array.Find(soundsEfeitos, sound => sound.nome == nome);


        if (s == null)
            return;
        if (!s.source.isPlaying || nome == "Temp_Tiro")
        {
            s.source.volume = s.volume * VolumeEfeito / 100;
            s.source.Play();
        }

        
    }
    public void PlayMusica(string nome)
    {
        if (nome != musicaAtual)
        {
            Sound temp = Array.Find(soundsMusicas, sound => sound.nome == musicaAtual);
            if(musicaAtual !="")
            temp.source.Stop();
            musicaAtual = nome;

        }
        Sound s = Array.Find(soundsMusicas, sound => sound.nome == nome);
        Debug.Log(s);
        


        if (s == null)
            return;
        s.source.volume = s.volume * VolumeMusica/100;
        s.source.Play();
    }

    public void AumentarVolumeEfeito(float valor)
    {
        
        VolumeEfeito = valor;
        PlayerPrefs.SetFloat("VolumeEfeito", VolumeEfeito);
        mudancaVolumeEfeitos();
    }
    public void AumentarVolumeMusica(float valor)
    {
        VolumeMusica = valor;
        PlayerPrefs.SetFloat("VolumeMusica", VolumeMusica);
        mudancaVolumeMusica();
    }
    public float RetornarValorEfeito()
    {
        mudancaVolumeEfeitos();
        return VolumeEfeito;
    }
    public float RetornarValorMusica()
    {
        mudancaVolumeMusica();
        return VolumeMusica;
        
    }

    private void mudancaVolumeMusica()
    {
        
        foreach (Sound s in soundsMusicas)
        {
           
            s.source.volume = s.volume * VolumeMusica / 100;
        }
    }
    private void mudancaVolumeEfeitos()
    {

        foreach (Sound s in soundsEfeitos)
        {
            
            s.source.volume = s.volume * VolumeEfeito / 100;
        }
    }
}
