<?php

namespace App;

use Illuminate\Database\Eloquent\Model;

class EventsHandling extends Model
{
    /**
    * Таблица, связанная с моделью.
    *
    * @var string
    */
    protected $table = 'Lom_EventsHandling';

  /**
    * @var bool
    */
    public $timestamps = false;

    /**
     * The attributes that are mass assignable.
     *
     * @var array
     */
    protected $fillable = [
      'id_People', 'id_EventType', 'HumanOrder',
  ];
}
