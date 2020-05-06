<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class People extends Model
{
  /**
    * Таблица, связанная с моделью.
    *
    * @var string
    */
    protected $table = 'Lom_People';

  /**
    * @var bool
    */
    public $timestamps = false;

   /**
     * The attributes that should be hidden for arrays.
     *
     * @var array
     */
    protected $hidden = [
        'Password', 'DateTimeCreate', 'ActivityStatus',
    ];

    /**
     * The attributes that are mass assignable.
     *
     * @var array
     */
    protected $fillable = [
      'SurName', 'FirstName', 'PatronymicName', 'Login', 'Password',
  ];
}
