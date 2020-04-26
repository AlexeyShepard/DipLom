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


}
